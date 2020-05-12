using System;
using System.Collections;
using UnityEngine;

using NineHundredLbs.UnitytoDMX.LED.Effects;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [Serializable]
    public class LEDDMXProperties : IDMXProperties
    {
        public LEDEffect ledEffect;

        public LEDDMXProperties(LEDEffect ledEffect)
        {
            this.ledEffect = ledEffect;
        }
    }

    public class LEDDMXController : ADMXController<LEDDMXProperties>
    {
        #region Constants
        /// <summary>
        /// Maximum LEDs per strip.
        /// </summary>
        private const int MAX_LED_COUNT = 128;
        #endregion

        #region Properties
        public int LEDCount => ledCount;

        public bool IsReversed => isReversed;
        #endregion

        #region Serialized Private Variables
        [Tooltip("The number of controlled LEDs.")]
        [Range(1, MAX_LED_COUNT), SerializeField] private int ledCount = 128;

        [Tooltip("Whether or not this LED controller is reversed.")]
        [SerializeField] private bool isReversed = false;
        #endregion

        #region Private Variables
        private Coroutine dispatchCoroutine;
        private Coroutine effectCoroutine;
        private byte[] dmxBytes;
        #endregion

        #region Public Methods
        public override byte[] GetDMXData()
        {
            if (dmxBytes == null)
                dmxBytes = new byte[ledCount * LEDEffectUtility.LEDByteCount];

            return dmxBytes;
        }

        public override void SendCommand(byte[] dmxData)
        {
            if (isReversed)
            {
                for (int i = 0; i < dmxData.Length / 2; i++)
                {
                    byte temp = dmxData[i];
                    dmxData[i] = dmxData[dmxData.Length - i - 1];
                    dmxData[dmxData.Length - i - 1] = temp;
                }

                for (int i = 0; i < dmxData.Length; i++)
                {
                    switch (i % LEDEffectUtility.LEDByteCount)
                    {
                        case 0:
                            byte outer = dmxData[i];
                            dmxData[i] = dmxData[i + 3];
                            dmxData[i + 3] = outer;
                            break;
                        case 1:
                            byte inner = dmxData[i];
                            dmxData[i] = dmxData[i + 1];
                            dmxData[i + 1] = inner;
                            break;
                        default:
                            break;
                    }
                }
            }
            base.SendCommand(dmxData);
        }

        public override void SendCommand(ArraySegment<byte> dmxData)
        {
            if (isReversed)
            {
                for (int i = dmxData.Offset; i < dmxData.Offset + (dmxData.Count / 2); i++)
                {
                    byte temp = dmxData.Array[i];
                    dmxData.Array[i] = dmxData.Array[(dmxData.Offset + dmxData.Count) - (i - dmxData.Offset) - 1];
                    dmxData.Array[(dmxData.Offset + dmxData.Count) - (i - dmxData.Offset) - 1] = temp;
                }

                for (int i = dmxData.Offset; i < dmxData.Offset + dmxData.Count; i++)
                {
                    switch (i % LEDEffectUtility.LEDByteCount)
                    {
                        case 0:
                            byte outer = dmxData.Array[i];
                            dmxData.Array[i] = dmxData.Array[i + 3];
                            dmxData.Array[i + 3] = outer;
                            break;
                        case 1:
                            byte inner = dmxData.Array[i];
                            dmxData.Array[i] = dmxData.Array[i + 1];
                            dmxData.Array[i + 1] = inner;
                            break;
                        default:
                            break;
                    }
                }
            }
            base.SendCommand(dmxData);
        }

        public void Clear()
        {
            if (effectCoroutine != null)
                StopCoroutine(effectCoroutine);

            if (dispatchCoroutine != null)
                StopCoroutine(dispatchCoroutine);

            LEDEffectUtility.WriteColorToBytes(Color.black, GetDMXData());
            SendCommand(GetDMXData());
        }
        #endregion

        #region Protected Methods
        protected override void OnPropertiesSet()
        {
            base.OnPropertiesSet();
            Clear();
            effectCoroutine = StartCoroutine(Properties.ledEffect.Execute(GetDMXData()));
            dispatchCoroutine = StartCoroutine(IE_Dispatch());

            IEnumerator IE_Dispatch()
            {
                while (true)
                {
                    SendCommand(GetDMXData());
                    yield return null;
                    yield return null;
                }
            }
        }
        #endregion
    }
}