using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct PathPoint
{
    public Vector3 Position;
    public Vector3 Direction;
    public Quaternion Rotation;
}

public class SmoothPathWithRotation : SmoothPath
{
    private List<Quaternion> _rotations = new List<Quaternion>();

    public SmoothPathWithRotation(PathPoint pointA, List<PathPoint> pointsBetween, PathPoint pointB, float smoothness) :
        base(pointA.Position, pointB.Position, pointsBetween.Select(point => point.Position).ToArray(), pointA.Direction, pointB.Direction, smoothness)
    {
        _rotations.Add(pointA.Rotation);

        foreach (var item in pointsBetween)
            _rotations.Add(item.Rotation);

        _rotations.Add(pointB.Rotation);
    }

    public void GetPositionAndRotation(float progress, out Vector3 position, out Quaternion rotation)
    {
        int i = GetIndex(progress);
        float partProgress = GetProgressForPart(progress, i);
        position = GetProgress(i, partProgress);
        rotation = Quaternion.Lerp(_rotations[i], _rotations[i + 1], partProgress);
    }
}
