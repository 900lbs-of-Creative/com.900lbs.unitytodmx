using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Glitch Effect", menuName = "DMX/LED/Create New Glitch Effect")]
    public class GlitchLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [SerializeField] private Gradient glitchColors = default;
        
        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                for (int i = 0; i < dmxData.Length - 1; i++)
                {
                    if (i % 4 == 0)
                    {
                        LEDEffectUtility.WriteColorToBytes(glitchColors.Evaluate(UnityEngine.Random.value), UnityEngine.Random.value,
                            new ArraySegment<byte>(dmxData, i, LEDEffectUtility.LEDByteCount));
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}