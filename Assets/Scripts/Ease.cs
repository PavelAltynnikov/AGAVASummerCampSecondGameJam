using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ease
{
    public static float EaseInEaseOut(float t)
    {
        return (Mathf.Sin((2 * t - 1) * Mathf.PI / 2) / 2) + 0.5f;
    }

    public static float EaseIn(float t)
    {
        return Mathf.Sin((t - 1) * Mathf.PI / 2) + 1;
    }

    public static float EaseOut(float t)
    {
        return Mathf.Sin(t * Mathf.PI / 2);
    }
}