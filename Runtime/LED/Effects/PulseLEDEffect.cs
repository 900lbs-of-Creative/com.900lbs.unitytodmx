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

        public void Init(float duration, Gradient colorOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
        }

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
                LEDEffectUtility.WriteColorToBytes(colorOverDuration.Evaluate(normalizedTime), dmxData);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}