using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Trail Loop Effect", menuName = "DMX/LED/Create New Trail Loop Effect")]
    public class TrailLoopLEDEffect : LEDEffect
    {
        [Header("Settings and Preferences")]
        [Tooltip("Percentage of the entire LED controller the trail takes up.")]
        [SerializeField, Range(0.01f, 0.99f)] private float percentageLength = 0.25f;

        [Tooltip("Relative speed of the trail with 1 being 60 units per second")]
        [SerializeField, Range(0.01f, 1.0f)] private float speed = 0.5f;

        [Tooltip("Color of the trail over the length.")]
        [SerializeField] private Gradient colorOverLength = default;

        public void Init(float percentageLength, float speed, Gradient colorOverLength)
        {
            this.percentageLength = percentageLength;
            this.speed = speed;
            this.colorOverLength = colorOverLength;
        }

        public static TrailLoopLEDEffect CreateInstance(float percentageLength, float speed, Gradient colorOverLength)
        {
            var instance = CreateInstance<TrailLoopLEDEffect>();
            instance.Init(percentageLength, speed, colorOverLength);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            int currentLEDIndex = 0;
            int ledTrailLength;
            bool looped = false;
            
            while (true)
            {
                LEDEffectUtility.WriteColorToBytes(Color.clear, dmxData);

                ledTrailLength = GetLEDTrailLength();

                for (int i = currentLEDIndex - ledTrailLength + 1; i <= currentLEDIndex; i++)
                {
                    int targetLEDIndex = i;
                    if (targetLEDIndex < 0)
                    {
                        if (!looped)
                            continue;

                        targetLEDIndex += GetLEDCount();
                    }

                    else if (targetLEDIndex >= GetLEDCount())
                    {
                        targetLEDIndex -= GetLEDCount();
                        if (!looped)
                            looped = true;
                    }

                    Color color = colorOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    LEDEffectUtility.WriteColorToBytes(color, new ArraySegment<byte>(dmxData, targetLEDIndex * LEDEffectUtility.LEDByteCount, LEDEffectUtility.LEDByteCount));
                }

                if (currentLEDIndex == GetLEDCount() + ledTrailLength)
                    currentLEDIndex = ledTrailLength;
                else
                    currentLEDIndex++;

                for (float i = 0; i < 60 / (speed * 60); i++)
                    yield return null;
            }

            int GetLEDTrailLength()
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