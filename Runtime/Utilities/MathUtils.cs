using UnityEngine;

public static class MathUtils
{
    /// <summary>
    /// Return a framerate independent dampening factor (-1 = instant)
    /// </summary>
    /// <param name="dampening">Dampening coefficient.</param>
    /// <param name="deltaTime">Delta time.</param>
    /// <returns></returns>
    public static float GetDampenFactor(float dampening, float deltaTime)
    {
        if (dampening < 0.0f)
            return 1.0f;
        if (Application.isPlaying == false)
            return 1.0f;

        return 1.0f - Mathf.Exp(-dampening * deltaTime);
    }
}