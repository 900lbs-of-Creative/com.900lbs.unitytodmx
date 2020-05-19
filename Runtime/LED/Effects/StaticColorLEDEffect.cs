using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    [CreateAssetMenu(fileName = "New Static Color Effect", menuName = "DMX/LED/Create New Static Color Effect")]
    public class StaticColorLEDEffect : LEDEffect
    {
        #region Serialized Private Variables
        [Header("Settings and Preferences")]
        [Tooltip("Target color.")]
        [SerializeField] private Color color = Color.white;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize with the given <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Target color.</param>
        public void Init(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Create and return a new instance with the given <paramref name="color"/>.
        /// </summary>
        /// <param name="color">Target color.</param>
        /// <returns></returns>
        public static StaticColorLEDEffect CreateInstance(Color color)
        {
            var instance = CreateInstance<StaticColorLEDEffect>();
            instance.Init(color);
            return instance;
        }

        public override IEnumerator Execute(byte[] dmxData)
        {
            while (true)
            {
                LEDEffectUtility.WriteColorToBytes(dmxData, color);
                yield return null;
            }
        }
        #endregion
    }
}
