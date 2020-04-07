using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Trail Effect", menuName = "DMX/LED/Create New Trail Effect")]
    public class TrailLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [Tooltip("Percentage of the entire LED controller the trail takes up.")]
        [SerializeField, Range(0.01f, 0.99f)] private float percentageLength = 0.25f;

        [Tooltip("Relative speed of the trail with 1 being 60 units per second")]
        [SerializeField, Range(0.01f, 1.0f)] private float speed = 0.5f;

        [Tooltip("Color of the trail over the length.")]
        [SerializeField] private Gradient colorOverLength = default;

        [Tooltip("Intensity of the trail over the length.")]
        [SerializeField] private AnimationCurve intensityOverLength = new AnimationCurve(new Keyframe(0.0f, 1.0f) , new Keyframe(1.0f, 0.0f));

        public override IEnumerator Execute(byte[] dmxData)
        {
            int currentLEDIndex = 0;
            int ledTrailLength = GetLEDTrailLength(dmxData);
            while (currentLEDIndex < GetLEDCount() + ledTrailLength)
            {
                ledTrailLength = GetLEDTrailLength(dmxData);
                // Color all lights black.
                LEDEffectUtility.WriteColorToBytes(Color.black, 1.0f, dmxData);

                for (int i = currentLEDIndex - ledTrailLength + 1; i <= currentLEDIndex; i++)
                {
                    if (i < 0 || i >= GetLEDCount())
                        continue;

                    Color color = colorOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    float intensity = intensityOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    LEDEffectUtility.WriteColorToBytes(color, intensity, new ArraySegment<byte>(dmxData, i * LEDEffectUtility.LEDByteCount, LEDEffectUtility.LEDByteCount));
                }

                currentLEDIndex++;

                for (float i = 0; i < 60 / (speed * 60); i++)
                    yield return null;
            }

            int GetLEDTrailLength(byte[] bytes)
            {
                return Mathf.CeilToInt(GetLEDCount() * percentageLength);
            }


            int GetLEDCount()
            {
                return Mathf.FloorToInt(dmxData.Length / LEDEffectUtility.LEDByteCount);
            }
        }
    }
}