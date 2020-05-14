using System;
using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    [CreateAssetMenu(fileName = "New Trail Effect", menuName = "DMX/LED/Create New Trail Effect")]
    public class TrailLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Percentage of the entire LED controller the trail takes up.")]
        [SerializeField, Range(0.01f, 0.99f)] private float percentageLength = 0.25f;

        [Tooltip("Relative speed of the trail with 1 being 60 units per second")]
        [SerializeField, Range(0.01f, 1.0f)] private float speed = 0.5f;

        [Tooltip("Color of the trail over the length.")]
        [SerializeField] private Gradient colorOverLength = default;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="percentageLength"/>, <paramref name="speed"/>, and <paramref name="colorOverLength"/>.
        /// </summary>
        /// <param name="percentageLength">Percentage of the entire LED controller the trail takes up.</param>
        /// <param name="speed">Relative speed of the trail with 1 being 60 units per second.</param>
        /// <param name="colorOverLength">Color of the trail over the length.</param>
        public void Init(float percentageLength, float speed, Gradient colorOverLength)
        {
            this.percentageLength = percentageLength;
            this.speed = speed;
            this.colorOverLength = colorOverLength;
        }

        /// <summary>
        /// Create and return a new instance with the given <paramref name="percentageLength"/>, <paramref name="speed"/>, and <paramref name="colorOverLength"/>.
        /// </summary>
        /// <param name="percentageLength">Percentage of the entire LED controller the trail takes up.</param>
        /// <param name="speed">Relative speed of the trail with 1 being 60 units per second.</param>
        /// <param name="colorOverLength">Color of the trail over the length.</param>
        /// <returns></returns>
        public static TrailLEDEffect CreateInstance(float percentageLength, float speed, Gradient colorOverLength)
        {
            var instance = CreateInstance<TrailLEDEffect>();
            instance.Init(percentageLength, speed, colorOverLength);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            int currentLEDIndex = 0;
            int ledTrailLength = GetLEDTrailLength(dmxData);
            while (currentLEDIndex < GetLEDCount() + ledTrailLength)
            {
                ledTrailLength = GetLEDTrailLength(dmxData);

                LEDEffectUtility.WriteColorToBytes(Color.clear, dmxData);

                for (int i = currentLEDIndex - ledTrailLength + 1; i <= currentLEDIndex; i++)
                {
                    if (i < 0 || i >= GetLEDCount())
                        continue;

                    Color color = colorOverLength.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(currentLEDIndex - ledTrailLength, currentLEDIndex, i)));
                    LEDEffectUtility.WriteColorToBytes(color, new ArraySegment<byte>(dmxData, i * LEDEffectUtility.LEDByteCount, LEDEffectUtility.LEDByteCount));
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
        #endregion
    }
}