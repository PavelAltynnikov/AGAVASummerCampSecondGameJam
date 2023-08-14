using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteColorAnimation : MonoBehaviour
{
    [SerializeField] private Color _secondColor;
    [SerializeField] private float _cycleTime;
    [SerializeField] private float _showTime;

    private float _alphaMult;
    private SpriteRenderer _renderer;
    private Color _startColor;
    private float _timer;
    private Coroutine _changingAlpha;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _startColor = _renderer.color;
        _timer = 0;
    }

    private void Update()
    {
        var lerp = Mathf.Abs(_timer / _cycleTime - 0.5f) * 2;
        var color = Color.Lerp(_startColor, _secondColor, lerp);
        color.a *= _alphaMult;
        _renderer.color = color;
        _timer += Time.deltaTime;
        if (_timer > _cycleTime)
        {
            _timer -= _cycleTime;
        }
    }

    public void Show()
    {
        if (_changingAlpha != null)
            StopCoroutine(_changingAlpha);
        _changingAlpha = StartCoroutine(ChangeAlpha(1));
    }

    public void Hide()
    {
        if (_changingAlpha != null)
            StopCoroutine(_changingAlpha);
        _changingAlpha = StartCoroutine(ChangeAlpha(0));
    }

    private IEnumerator ChangeAlpha(float target)
    {
        var timer = 0f;
        var startAlpha = _alphaMult;
        while (timer < _showTime)
        {
            _alphaMult = Mathf.Lerp(startAlpha, target, timer / _showTime);
            yield return null;
            timer += Time.deltaTime;
        }
    }



}
