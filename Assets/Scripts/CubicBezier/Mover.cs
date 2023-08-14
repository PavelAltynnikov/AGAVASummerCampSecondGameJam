using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _distance = 5;

    private Vector3 _startPosition;
    private CubicBezier _cubicBezier = new CubicBezier(Easing.EaseInOut);
    private float _time = 0;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float value = _cubicBezier.GetValue(_time / _duration);
        transform.position = Vector3.LerpUnclamped(_startPosition, _startPosition + Vector3.forward * _distance, value);
        _time += Time.deltaTime;

        if (_time > _duration)
            _time = 0;
    }
}
