using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Pulse Effect", menuName = "DMX/LED/Create New Pulse Effect")]
    public class PulseLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [Tooltip("Duration of the pulse.")]
        [SerializeField] private float duration = 0.5f;

        [Tooltip("Color of the pulse over the duration.")]
        [SerializeField] private Gradient colorOverDuration = default;

        [Tooltip("Intensity of the pulse over the duration.")]
        [SerializeField] private AnimationCurve intensityOverDuration = new AnimationCurve(new Keyframe(0.0f, 0.0f), new Keyframe(0.5f, 1.0f), new Keyframe(1.0f, 0.0f));

        public void Init(float duration, Gradient colorOverDuration, AnimationCurve intensityOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
            this.intensityOverDuration = intensityOverDuration;
        }

        public static PulseLEDEffect CreateInstance(float duration, Gradient colorOverDuration, AnimationCurve intensityOverDuration)
        {
            var instance = CreateInstance<PulseLEDEffect>();
            instance.Init(duration, colorOverDuration, intensityOverDuration);
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
                LEDEffectUtility.WriteColorToBytes(colorOverDuration.Evaluate(normalizedTime), intensityOverDuration.Evaluate(normalizedTime), dmxData);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}