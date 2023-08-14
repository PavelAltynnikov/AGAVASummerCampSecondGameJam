using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatedPathVisualization : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _smoothPower = 1;
    [SerializeField] private GameObject _movedObject;
    [SerializeField] private float _duration = 2;

    private float _time = 0;
    private SmoothPathWithRotation _path;

    private void OnDrawGizmos()
    {
        if (_path == null)
            UpdatePath();

        for (float t = 0; t < 1; t += 0.02f)
            Gizmos.DrawSphere(_path.GetPosition(t), 0.05f);
    }

    private void UpdatePath()
    {
        if (_points.Length < 2)
            return;

        Transform firstPoint = _points[0];
        Transform lastPoint = _points[_points.Length - 1];
        List<PathPoint> pointsBetween = new List<PathPoint>();
        for (int i = 1; i < _points.Length - 1; i++)
            pointsBetween.Add(CreatePoint(_points[i]));


        _path = new SmoothPathWithRotation(CreatePoint(firstPoint), pointsBetween, CreatePoint(lastPoint), _smoothPower);
    }

    private PathPoint CreatePoint(Transform transform)
    {
        PathPoint point = new PathPoint();
        point.Position = transform.position;
        point.Rotation = transform.rotation;
        point.Direction = transform.forward;
        return point;
    }

    private void Update()
    {
        _path.GetPositionAndRotation(_time / _duration, out Vector3 newPosition, out Quaternion newRotation);
        _movedObject.transform.position = newPosition;
        _movedObject.transform.rotation = newRotation;

        _time += Time.deltaTime;
        if (_time > _duration)
            _time = 0;
    }

    private void FixedUpdate()
    {
        UpdatePath();
    }
}
