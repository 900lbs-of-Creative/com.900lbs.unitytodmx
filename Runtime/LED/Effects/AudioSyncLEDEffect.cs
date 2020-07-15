using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Audio Sync Effect", menuName = "DMX/LED/Create New Audio Sync Effect")]
    public class AudioSyncLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Colors to change to based on sampled audio value.")]
        [SerializeField] private Gradient colors = default;

        [Tooltip("How quickly colors change to match the currently sampled audio values.\n" +
            "-1 = Instantly change.\n" +
            "1 = Slowly change.\n" +
            "10 = Quickly change.")]
        [SerializeField] private float colorDampening = -1;
        #endregion

        #region Private Variables
        private float[] samples = new float[512];
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="colors"/> and <paramref name="colorDampening"/>.
        /// </summary>
        /// <param name="colors">Colors to change to based on sampled audio value.</param>
        /// <param name="colorDampening">How quickly colors change to match the currently sampled audio values. -1 = Instantly change. 1 = Slowly change. 10 = Quickly change.</param>
        public void Init(Gradient colors, float colorDampening)
        {
            this.colors = colors;
            this.colorDampening = colorDampening;
        }

        /// <summary>
        /// Create and return an instance with the given <paramref name="colors"/> and <paramref name="colorDampening"/>.
        /// </summary>
        /// <param name="colors">Colors to change to based on sampled audio value.</param>
        /// <param name="colorDampening">How quickly colors change to match the currently sampled audio values. -1 = Instantly change. 1 = Slowly change. 10 = Quickly change.</param>
        /// <returns></returns>
        public static AudioSyncLEDEffect CreateInstance(Gradient colors, float colorDampening)
        {
            var instance = CreateInstance<AudioSyncLEDEffect>();
            instance.Init(colors, colorDampening);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                AudioListener.GetSpectrumData(samples, 0, FFTWindow.Blackman);
                for (int i = 0; i < dmxData.Length; i += 4)
                {
                    int remappedIndex = Mathf.FloorToInt(Mathf.Lerp(0, samples.Length, Mathf.InverseLerp(0, dmxData.Length, i)));
                    var currentColor = LEDEffectUtils.ConvertBytesToColor(new ArraySegment<byte>(dmxData, i, LEDEffectUtils.LEDByteCount));
                    var targetColor = colors.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, 0.01f, samples[remappedIndex])));

                    LEDEffectUtils.WriteColorToBytes(
                        new ArraySegment<byte>(dmxData, i, LEDEffectUtils.LEDByteCount),
                        Color.Lerp(currentColor, targetColor, MathUtils.GetDampenFactor(colorDampening, Time.deltaTime)));
                }
                yield return null;
                yield return null;
            }
        }
        #endregion
    }
}