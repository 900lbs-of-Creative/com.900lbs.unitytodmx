using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Static Color Effect", menuName = "DMX/LED/Create New Static Color Effect")]
    public class StaticColorLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [SerializeField] private Color32 color = Color.white;
        [SerializeField] private float intensity = 1.0f;

        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                LEDEffectUtility.WriteColorToBytes(color, intensity, dmxData);
                yield return null;
            }
        }
    }
}
