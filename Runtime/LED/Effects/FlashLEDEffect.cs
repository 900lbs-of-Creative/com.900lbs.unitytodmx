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

        public void Init(float duration, Gradient colorOverDuration)
        {
            this.duration = duration;
            this.colorOverDuration = colorOverDuration;
        }

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
                //Debug.Log(colorOverDuration.Evaluate(normalizedTime));
                LEDEffectUtility.WriteColorToBytes(colorOverDuration.Evaluate(normalizedTime), dmxData);
                timer += Time.deltaTime;
                yield return null;
            }
            LEDEffectUtility.WriteColorToBytes(Color.clear, dmxData);
        }
    }
}
