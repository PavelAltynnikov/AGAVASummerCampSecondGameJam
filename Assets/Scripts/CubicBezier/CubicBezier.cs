using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Easing
{
    EaseIn,
    EaseOut,
    EaseInOut
}

public class CubicBezier
{
    private Vector2 _p0 = Vector2.zero;
    private Vector2 _p3 = Vector2.one;
    private Vector2 _p1;
    private Vector2 _p2;

    public CubicBezier(float p1x, float p1y, float p2x, float p2y)
    {
        _p1 = new Vector2(p1x, p1y);
        _p2 = new Vector2(p2x, p2y);
    }

    public CubicBezier(Easing curve)
    {
        switch (curve)
        {
            case Easing.EaseIn:
                _p1 = new Vector2(0.5f, 0);
                _p2 = new Vector2(1, 0.5f);
                break;
            case Easing.EaseOut:
                _p1 = new Vector2(0, 0.5f);
                _p2 = new Vector2(0.5f, 1);
                break;
            case Easing.EaseInOut:
                _p1 = new Vector2(0.5f, 0);
                _p2 = new Vector2(0.5f, 1);
                break;
        }
    }

    public CubicBezier(Vector2 p1, Vector2 p2)
    {
        _p1 = p1;
        _p2 = p2;
    }

    public float GetValue(float t)
    {
        return GetPoint(t).y;
    }

    private Vector2 GetPoint(float t)
    {
        return Mathf.Pow(1 - t, 3) * _p0 +
                3 * Mathf.Pow(1 - t, 2) * t * _p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * _p2 +
                Mathf.Pow(t, 3) * _p3;
    }
}
