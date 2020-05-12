using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Static Color Effect", menuName = "DMX/LED/Create New Static Color Effect")]
    public class StaticColorLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [SerializeField] private Color32 color = Color.white;

        public void Init(Color32 color)
        {
            this.color = color;
        }

        public static StaticColorLEDEffect CreateInstance(Color32 color)
        {
            var instance = CreateInstance<StaticColorLEDEffect>();
            instance.Init(color);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                LEDEffectUtility.WriteColorToBytes(color, dmxData);
                yield return null;
            }
        }
    }
}
