using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Glitch Effect", menuName = "DMX/LED/Create New Glitch Effect")]
    public class GlitchLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Colors sampled from when glitching.")]
        [SerializeField] private Gradient colors = default;

        [Tooltip("Intensity curve sampled from when glitching.")]
        [SerializeField] private AnimationCurve intensity = default;

        [Tooltip("Duration of the glitch (-1 loops infinitely).")]
        [SerializeField] private float duration = default;

        [Tooltip("Speed of glitch per second.")]
        [SerializeField] private float speed = default;

        [Tooltip("How often each pixel changes to sampled glitch color.\n" +
            "0.1 = Slowly change.\n" +
            "1 = Instantly change.")]
        [SerializeField, Range(0.1f, 1.0f)] private float deviation = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="colors"/>, <paramref name="intensity"/>,
        /// <paramref name="duration"/>, <paramref name="speed"/>, and <paramref name="deviation"/>.
        /// </summary>
        /// <param name="colors">Colors sampled from when glitching.</param>
        /// <param name="intensity">Intensity curve sampled from when glitching.</param>
        /// <param name="duration">Duration of the glitch (-1 loops infinitely).</param>
        /// <param name="speed">Speed of glitch per second.</param>
        /// <param name="deviation">How often each pixel changes to sampled glitch color. 0 = Slowly change. 1 = Instantly change.</param>
        public void Init(Gradient colors, AnimationCurve intensity, float duration, float speed, float deviation)
        {
            this.colors = colors;
            this.intensity = intensity;
            this.duration = duration;
            this.speed = speed;
            this.deviation = deviation;
        }

        /// <summary>
        /// Create and return an instance with the given <paramref name="colors"/>, <paramref name="intensity"/>,
        /// <paramref name="duration"/>, <paramref name="speed"/>, and <paramref name="deviation"/>
        /// </summary>
        /// <param name="colors">Colors sampled from when glitching.</param>
        /// <param name="intensity">Intensity curve sampled from when glitching.</param>
        /// <param name="duration">Duration of the glitch (-1 loops infinitely).</param>
        /// <param name="speed">Speed of glitch per second.</param>
        /// <param name="deviation">How often each pixel changes to sampled glitch color. 0 = Slowly change. 1 = Instantly change.</param>
        /// <returns></returns>
        public static GlitchLEDEffect CreateInstance(Gradient colors, AnimationCurve intensity, float duration, float speed, float deviation)
        {
            var instance = CreateInstance<GlitchLEDEffect>();
            instance.Init(colors, intensity, duration, speed, deviation);
            return instance;
        }
        
        public override IEnumerator Execute(byte[] dmxData)
        {
            if (duration == -1)
            {
                while (true)
                {
                    for (int i = 0; i < dmxData.Length - 1; i += 4)
                    {
                        if (1 - Random.value > deviation)
                            continue;

                        LEDEffectUtility.WriteColorToBytes(new ArraySegment<byte>(dmxData, i, LEDEffectUtility.LEDByteCount), colors.Evaluate(Random.value) * intensity.Evaluate(Random.value));
                    }

                    for (int i = 0; i < 60 / speed; i++)
                        yield return null;
                }
            }
            else
            {
                float timer = 0.0f;
                while (timer <= duration)
                {
                    for (int i = 0; i < dmxData.Length - 1; i += 4)
                    {
                        if (1 - Random.value > deviation)
                            continue;

                        LEDEffectUtility.WriteColorToBytes(new ArraySegment<byte>(dmxData, i, LEDEffectUtility.LEDByteCount), colors.Evaluate(Random.value) * intensity.Evaluate(Random.value));
                    }

                    for (int i = 0; i < 60 / speed; i++)
                    {
                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
                LEDEffectUtility.WriteColorToBytes(dmxData, Color.clear);
            }
        }
        #endregion
    }
}