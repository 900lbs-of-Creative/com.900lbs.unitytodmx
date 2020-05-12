using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Flash Effect", menuName = "DMX/LED/Create New Flash Effect")]
    public class FlashLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [Tooltip("Duration of the flash.")]
        [SerializeField] private float duration = 0.5f;

        [Tooltip("Color of the flash over the duration.")]
        [SerializeField] private Gradient colorOverDuration = default;

        [Tooltip("Intensity of the flash over the duration.")]
        [SerializeField] private AnimationCurve intensityOverDuration = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f));

        public void Init(float duration, Gradient colorOverDuration, AnimationCurve intensityOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
            this.intensityOverDuration = intensityOverDuration;
        }

        public static FlashLEDEffect CreateInstance(float duration, Gradient colorOverDuration, AnimationCurve intensityOverDuration)
        {
            var instance = CreateInstance<FlashLEDEffect>();
            instance.Init(duration, colorOverDuration, intensityOverDuration);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            float timer = 0.0f;
            while (timer < duration)
            {
                float normalizedTime = timer / duration;
                LEDEffectUtility.WriteColorToBytes(colorOverDuration.Evaluate(normalizedTime), intensityOverDuration.Evaluate(normalizedTime), dmxData);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
