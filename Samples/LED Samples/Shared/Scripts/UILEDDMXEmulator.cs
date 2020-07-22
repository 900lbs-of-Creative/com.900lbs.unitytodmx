using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NineHundredLbs.UnitytoDMX.LED.Examples
{
    public class UILEDDMXEmulator : LEDDMXEmulator
    {
        [Header("Object References")]
        [SerializeField] private List<Image> uiLEDs = default;

        private Color32[] colors = new Color32[128];

        public override Color32[] GetColors()
        {
            return colors;
        }

        protected override void UpdateLEDEmulation(Color32[] colors)
        {
            for (int i = 0; i < uiLEDs.Count; i++)
            {
                if (colors.Length <= i)
                    break;

                if (uiLEDs[i] == null)
                    continue;

                uiLEDs[i].color = colors[i];
            }
        }
    }
}
