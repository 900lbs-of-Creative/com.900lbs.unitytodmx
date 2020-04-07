using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED.Effects
{
    #region Interfaces
    public interface ILEDEffect
    {
        IEnumerator Execute(byte[] dmxData);
    }
    #endregion

    public abstract class LEDEffect : ScriptableObject, ILEDEffect
    {
        public abstract IEnumerator Execute(byte[] dmxData);
    }
}
