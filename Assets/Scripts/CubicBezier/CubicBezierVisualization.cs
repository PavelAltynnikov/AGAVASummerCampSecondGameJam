using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicBezierVisualization : MonoBehaviour
{
    [SerializeField] private Transform _point1;
    [SerializeField] private Transform _point2;
    [SerializeField] private GameObject _moverPrefab;
    [SerializeField] private float _duration = 2;

    private GameObject _mover;
    private float _time = 0;

    private void OnDrawGizmos()
    {
        float scaleX = transform.lossyScale.x;
        float scaleY = transform.lossyScale.y;
        Vector3 point0 = transform.position;
        Vector3 point3 = transform.position + new Vector3(scaleX, scaleY, 0);

        CubicBezier bezier = CreateCubicBezier();
        Gizmos.DrawSphere(point0, 0.2f);
        Gizmos.DrawSphere(point3, 0.2f);
        for (float t = 0; t <= 1; t += 0.05f)
            Gizmos.DrawSphere(transform.position + new Vector3(t * scaleX, bezier.GetValue(t) * scaleY, 0), 0.1f);

        Gizmos.DrawLine(point0, _point1.position);
        Gizmos.DrawLine(point3, _point2.position);
    }

    private void Awake()
    {
        _mover = Instantiate(_moverPrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        float value = CreateCubicBezier().GetValue(_time / _duration);
        _mover.transform.position = Vector3.LerpUnclamped(transform.position, transform.position + Vector3.right * transform.lossyScale.x, value);
        _time += Time.deltaTime;

        if (_time > _duration)
            _time = 0;

        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log(_point1.localPosition.x.ToString() + ", " +
                _point1.localPosition.y.ToString() + ", " +
                _point2.localPosition.x.ToString() + ", " +
                _point2.localPosition.y.ToString());
    }

    private CubicBezier CreateCubicBezier()
    {
        return new CubicBezier(_point1.localPosition, _point2.localPosition);
    }
}
