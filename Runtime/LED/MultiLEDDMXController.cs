using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    public class MultiLEDDMXController : MonoBehaviour
    {
        #region Properties
        public LEDEffect Properties => properties;
        #endregion

        #region Serialized Private Variables
        [SerializeField] private LEDEffect properties = default;
        [SerializeField] private List<LEDDMXController> ledDMXControllers = default;
        #endregion

        #region Private Variables
        private byte[] multiLEDDMXData;
        private Coroutine dispatchCoroutine;
        private Coroutine effectCoroutine;
        #endregion

        #region Public Methods
        public void SetProperties(LEDEffect properties)
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
                int dmxDataCount = ledDMXController.LEDCount * LEDEffectUtils.LEDByteCount;
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

            LEDEffectUtils.WriteColorToBytes(GetDMXData(), Color.clear);
            SendCommand(GetDMXData());
        }
        #endregion

        #region Protected Methods
        private void OnPropertiesSet()
        {
            Clear();
            effectCoroutine = StartCoroutine(Properties.Execute(GetDMXData()));
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