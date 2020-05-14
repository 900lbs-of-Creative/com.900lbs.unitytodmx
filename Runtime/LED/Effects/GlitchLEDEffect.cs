using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Glitch Effect", menuName = "DMX/LED/Create New Glitch Effect")]
    public class GlitchLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Colors sampled from when glitching.")]
        [SerializeField] private Gradient glitchColors = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="glitchColors"/>.
        /// </summary>
        /// <param name="glitchColors">Colors sampled from when glitching.</param>
        public void Init(Gradient glitchColors)
        {
            this.glitchColors = glitchColors;
        }

        /// <summary>
        /// Create and return an instance with the given <paramref name="glitchColors"/>.
        /// </summary>
        /// <param name="glitchColors">Colors sampled from when glitching.</param>
        /// <returns></returns>
        public static GlitchLEDEffect CreateInstance(Gradient glitchColors)
        {
            var instance = CreateInstance<GlitchLEDEffect>();
            instance.Init(glitchColors);
            return instance;
        }
        
        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                for (int i = 0; i < dmxData.Length - 1; i++)
                {
                    if (i % 4 == 0)
                        LEDEffectUtility.WriteColorToBytes(glitchColors.Evaluate(UnityEngine.Random.value), new ArraySegment<byte>(dmxData, i, LEDEffectUtility.LEDByteCount));
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        #endregion
    }
}