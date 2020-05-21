using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Trail Effect", menuName = "DMX/LED/Create New Trail Effect")]
    public class TrailLEDEffect : LEDEffect
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
        public static TrailLEDEffect CreateInstance(float percentageLength, int speed, Gradient colorOverLength)
        {
            var instance = CreateInstance<TrailLEDEffect>();
            instance.Init(percentageLength, speed, colorOverLength);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            int currentLEDIndex;
            float currentLEDValue = 0.0f;
            int ledTrailLength;
            do
            {
                LEDEffectUtils.WriteColorToBytes(dmxData, Color.clear);

                ledTrailLength = GetLEDTrailLength();
                currentLEDIndex = (int)currentLEDValue;
                for (int i = currentLEDIndex - ledTrailLength; i <= currentLEDIndex; i++)
                {
                    if (i < 0 || i >= GetLEDCount())
                        continue;

                    Color color = colorOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    LEDEffectUtils.WriteColorToBytes(new ArraySegment<byte>(dmxData, i * LEDEffectUtils.LEDByteCount, LEDEffectUtils.LEDByteCount), color);
                }

                currentLEDValue += (speed * Time.deltaTime);
                yield return null;
            } while (currentLEDIndex < GetLEDCount() + ledTrailLength);

            int GetLEDTrailLength()
            {
                return Mathf.CeilToInt(GetLEDCount() * percentageLength);
            }

            int GetLEDCount()
            {
                return Mathf.FloorToInt(dmxData.Length / LEDEffectUtils.LEDByteCount);
            }
        }
        #endregion
    }
}