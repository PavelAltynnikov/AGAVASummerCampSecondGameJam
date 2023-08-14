using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCircleScaler : MonoBehaviour
{
    [SerializeField] private float _scaleMult;
    [SerializeField] private float _cycleTime;
    
    private Vector3 _startScale;
    private float _timer;

    private void Awake()
    {
        _startScale = transform.localScale;
        _timer = 0;
    }

    private void Update()
    {
        var lerpPoint = Mathf.Abs(_timer / _cycleTime - 0.5f) * 2;
        transform.localScale = Vector3.Lerp(_startScale * _scaleMult, _startScale / _scaleMult, lerpPoint);
        _timer += Time.deltaTime;
        if (_timer > _cycleTime)
        {
            _timer -= _cycleTime;
        }
    }
}
