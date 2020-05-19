using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Trail Loop Effect", menuName = "DMX/LED/Create New Trail Loop Effect")]
    public class TrailLoopLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Percentage of the entire LED controller the trail takes up.")]
        [SerializeField, Range(0.01f, 0.99f)] private float percentageLength = 0.25f;

        [Tooltip("Number of LEDs traversed per second.")]
        [SerializeField] private int speed = 64;

        [Tooltip("Color of the trail over the length.")]
        [SerializeField] private Gradient colorOverLength = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="percentageLength"/>, <paramref name="speed"/>, and <paramref name="colorOverLength"/>.
        /// </summary>
        /// <param name="percentageLength">Percentage of the entire LED controller the trail takes up.</param>
        /// <param name="speed">Number of LEDs traversed per second.</param>
        /// <param name="colorOverLength">Color of the trail over the length.</param>
        public void Init(float percentageLength, int speed, Gradient colorOverLength)
        {
            this.percentageLength = percentageLength;
            this.speed = speed;
            this.colorOverLength = colorOverLength;
        }

        /// <summary>
        /// Create and return a new instance with the given <paramref name="percentageLength"/>, <paramref name="speed"/>, and <paramref name="colorOverLength"/>.
        /// </summary>
        /// <param name="percentageLength">Percentage of the entire LED controller the trail takes up.</param>
        /// <param name="speed">Number of LEDs traversed per second.</param>
        /// <param name="colorOverLength">Color of the trail over the length.</param>
        /// <returns></returns>
        public static TrailLoopLEDEffect CreateInstance(float percentageLength, int speed, Gradient colorOverLength)
        {
            var instance = CreateInstance<TrailLoopLEDEffect>();
            instance.Init(percentageLength, speed, colorOverLength);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            int currentLEDIndex;
            float currentLEDValue = 0.0f;
            int ledTrailLength;
            bool looped = false;
            
            while (true)
            {
                LEDEffectUtility.WriteColorToBytes(dmxData, Color.clear);

                ledTrailLength = GetLEDTrailLength();
                currentLEDIndex = (int)currentLEDValue;

                for (int i = currentLEDIndex - ledTrailLength; i <= currentLEDIndex; i++)
                {
                    int targetLEDIndex = i;
                    if (targetLEDIndex < 0)
                    {
                        if (!looped)
                            continue;

                        targetLEDIndex = GetLEDCount() + targetLEDIndex;
                    }

                    else if (targetLEDIndex >= GetLEDCount())
                    {
                        if (!looped)
                            looped = true;

                        targetLEDIndex -= GetLEDCount();
                    }

                    Color color = colorOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    LEDEffectUtility.WriteColorToBytes(new ArraySegment<byte>(dmxData, targetLEDIndex * LEDEffectUtility.LEDByteCount, LEDEffectUtility.LEDByteCount), color);
                }

                if (currentLEDValue >= GetLEDCount() + ledTrailLength)
                    currentLEDValue = ledTrailLength;
                else
                    currentLEDValue += speed * Time.deltaTime;
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
        #endregion
    }
}