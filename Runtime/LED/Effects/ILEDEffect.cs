using System.Collections;
using UnityEngine;

namespace NineHundredLbs.UnitytoDMX.LED
{
    #region Interfaces
    /// <summary>
    /// Interface that DMX LED effects must implement.
    /// </summary>
    public interface ILEDEffect
    {
        IEnumerator Execute(byte[] dmxData);
    }
    #endregion

    /// <summary>
    /// Default implementation of a DMX LED effect.
    /// </summary>
    public abstract class LEDEffect : ScriptableObject, ILEDEffect
    {
        public abstract IEnumerator Execute(byte[] dmxData);
    }
}
