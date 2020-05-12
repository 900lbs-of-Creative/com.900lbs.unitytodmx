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

        public void Init(Color32 color, float intensity)
        {
            this.color = color;
            this.intensity = intensity;
        }

        public static StaticColorLEDEffect CreateInstance(Color32 color, float intensity)
        {
            var instance = CreateInstance<StaticColorLEDEffect>();
            instance.Init(color, intensity);
            return instance;
        }

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
