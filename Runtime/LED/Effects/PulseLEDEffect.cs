using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Pulse Effect", menuName = "DMX/LED/Create New Pulse Effect")]
    public class PulseLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Duration of the pulse.")]
        [SerializeField] private float duration = 1.0f;

        [Tooltip("Color of the pulse over the duration.")]
        [SerializeField] private Gradient colorOverDuration = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="duration"/> and <paramref name="colorOverDuration"/>.
        /// </summary>
        /// <param name="duration">Duration of the pulse.</param>
        /// <param name="colorOverDuration">Color of the pulse over the duration.</param>
        public void Init(float duration, Gradient colorOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
        }

        /// <summary>
        /// Create and return a new instance with the given <paramref name="duration"/> and <paramref name="colorOverDuration"/>.
        /// </summary>
        /// <param name="duration">Duration of the pulse.</param>
        /// <param name="colorOverDuration">Color of the pulse over the duration.</param>
        /// <returns></returns>
        public static PulseLEDEffect CreateInstance(float duration, Gradient colorOverDuration)
        {
            var instance = CreateInstance<PulseLEDEffect>();
            instance.Init(duration, colorOverDuration);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            float timer = 0.0f;
            while (true)
            {
                if (timer >= duration)
                    timer = 0.0f;

                float normalizedTime = timer / duration;
                LEDEffectUtils.WriteColorToBytes(dmxData, colorOverDuration.Evaluate(normalizedTime));
                timer += Time.deltaTime;
                yield return null;
            }
        }
        #endregion
    }
}