using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NineHundredLbs.UnitytoDMX.LED.Effects;

namespace NineHundredLbs.UnitytoDMX.LED
{
    public class MultiLEDDMXController : MonoBehaviour
    {
        #region Properties
        public LEDDMXProperties Properties => properties;
        #endregion

        #region Serialized Private Variables
        [SerializeField] private LEDDMXProperties properties = default;
        [SerializeField] private List<LEDDMXController> ledDMXControllers = default;
        #endregion

        #region Private Variables
        private byte[] multiLEDDMXData;
        private Coroutine dispatchCoroutine;
        private Coroutine effectCoroutine;
        #endregion

        #region Public Methods
        public void SetProperties(LEDDMXProperties properties)
        {
            this.properties = properties;
            OnPropertiesSet();
        }

        public byte[] GetDMXData()
        {
            if (multiLEDDMXData == null)
            {
                List<byte> multiDMXDataList = new List<byte>();
                foreach (var ledDMXController in ledDMXControllers)
                    multiDMXDataList.AddRange(ledDMXController.GetDMXData());
                multiLEDDMXData = multiDMXDataList.ToArray();
            }
            return multiLEDDMXData;
        }

        public void SendCommand(byte[] dmxData)
        {
            int currentDataIndex = 0;
            foreach (var ledDMXController in ledDMXControllers)
            {
                int dmxDataCount = ledDMXController.LEDCount * LEDEffectUtility.LEDByteCount;
                ledDMXController.SendCommand(new ArraySegment<byte>(dmxData, currentDataIndex, dmxDataCount));
                currentDataIndex += dmxDataCount;
            }
        }

        public void Clear()
        {
            if (effectCoroutine != null)
                StopCoroutine(effectCoroutine);

            if (dispatchCoroutine != null)
                StopCoroutine(dispatchCoroutine);

            LEDEffectUtility.WriteColorToBytes(Color.black, 1.0f, GetDMXData());
            SendCommand(GetDMXData());
        }
        #endregion

        #region Protected Methods
        private void OnPropertiesSet()
        {
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