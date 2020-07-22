using System;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Examples
{
    public abstract class LEDDMXEmulator : MonoBehaviour
    {
        public int Universe => universe;

        [Tooltip("Universe this emulator will emulating.")]
        [SerializeField] private int universe = default;

        public virtual void UpdateEmulation(ArraySegment<byte> arraySegment)
        {
            var colors = GetColors();
            LEDEffectUtils.TryConvertBytesToColors(arraySegment, ref colors);
            UpdateLEDEmulation(colors);
        }
        public abstract Color32[] GetColors();

        protected abstract void UpdateLEDEmulation(Color32[] colors);
    }
}
