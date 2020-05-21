using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Flash Effect", menuName = "DMX/LED/Create New Flash Effect")]
    public class FlashLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration = 1.0f;

        [Tooltip("Color of the flash over the duration.")]
        [SerializeField] private Gradient colorOverDuration = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="duration"/> and <paramref name="colorOverDuration"/>.
        /// </summary>
        /// <param name="duration">Duration of the flash.</param>
        /// <param name="colorOverDuration">Color of the flash over the duration.</param>
        public void Init(float duration, Gradient colorOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
        }

        /// <summary>
        /// Create and return a new instance with the given <paramref name="duration"/> and <paramref name="colorOverDuration"/>.
        /// </summary>
        /// <param name="duration">Duration of the flash.</param>
        /// <param name="colorOverDuration">Color of the flash over the duration.</param>
        /// <returns></returns>
        public static FlashLEDEffect CreateInstance(float duration, Gradient colorOverDuration)
        {
            var instance = CreateInstance<FlashLEDEffect>();
            instance.Init(duration, colorOverDuration);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            float timer = 0.0f;
            while (timer <= duration)
            {
                float normalizedTime = timer / duration;
                LEDEffectUtils.WriteColorToBytes(dmxData, colorOverDuration.Evaluate(normalizedTime));
                timer += Time.deltaTime;
                yield return null;
            }
            LEDEffectUtils.WriteColorToBytes(dmxData, Color.clear);
        }
        #endregion
    }
}
