using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothPathVisualization : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private float _smoothPower = 1;
    [SerializeField] private GameObject _moverPrefab;
    [SerializeField] private float _moveDuration = 2;

    private GameObject _mover;
    private SmoothPath _path;
    private float _time = 0;
    private CubicBezier easing = new CubicBezier(Easing.EaseInOut);

    private void OnDrawGizmos()
    {
        if (_path == null)
            UpdatePath();

        for (float t = 0; t < 1; t += 0.02f)
            Gizmos.DrawSphere(_path.GetPosition(t), 0.05f);
    }

    private void Start()
    {
        _mover = Instantiate(_moverPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        Vector3 lastPosition = _mover.transform.position;
        Vector3 newPosition = _path.GetPosition(easing.GetValue(_time / _moveDuration));

        _mover.transform.position = newPosition;
        _mover.transform.rotation = Quaternion.LookRotation(newPosition - lastPosition);
        _time += Time.deltaTime;

        if (_time > _moveDuration)
            _time = 0;
    }

    private void FixedUpdate()
    {
        UpdatePath();
    }

    private void UpdatePath()
    {
        if (_points.Length < 2)
            return;

        Transform firstPoint = _points[0];
        Transform lastPoint = _points[_points.Length - 1];
        List<Vector3> pointsBetween = new List<Vector3>();
        for (int i = 1; i < _points.Length - 1; i++)
            pointsBetween.Add(_points[i].position);

        _path = new SmoothPath(firstPoint.position, lastPoint.position, pointsBetween.ToArray(), firstPoint.transform.forward, lastPoint.transform.forward, _smoothPower);
    }
}
