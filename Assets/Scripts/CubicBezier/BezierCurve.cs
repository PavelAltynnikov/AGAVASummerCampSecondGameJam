using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve
{
    private Vector3 _p0;
    private Vector3 _p1;
    private Vector3 _p2;
    private Vector3 _p3;

    private Vector3 _v1;
    private Vector3 _v2;
    private Vector3 _v3;

    public float Length { get; private set; }

    public BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        _p0 = p0;
        _p1 = p1;
        _p2 = p2;
        _p3 = p3;

        Length = GetLength();

        _v1 = -3f * _p0 + 9f * _p1 - 9f * _p2 + 3f * _p3;
        _v2 = 6f * _p0 - 12f * _p1 + 6f * _p2;
        _v3 = -3f * _p0 + 3f * _p1;
    }

    public Vector3 GetPoint(float t)
    {
        return GetPointDitry(t);
    }

    private Vector3 GetPointDitry(float t)
    {
        return Mathf.Pow(1 - t, 3) * _p0 +
                3 * Mathf.Pow(1 - t, 2) * t * _p1 +
                3 * (1 - t) * Mathf.Pow(t, 2) * _p2 +
                Mathf.Pow(t, 3) * _p3;
    }

    private float GetLength()
    {
        float length = 0;
        Vector3 lastPoint = _p0;
        for (float t = 0; t < 1; t += 0.02f)
        {
            Vector3 point = GetPointDitry(t);
            length += Vector3.Distance(point, lastPoint);
            lastPoint = point;
        }
        return length;
    }
}
