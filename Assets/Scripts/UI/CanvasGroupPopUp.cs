using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupPopUp : MonoBehaviour
{
    [SerializeField] private float _time;
    [SerializeField] private bool _changeInteractions;

    private bool _active = false;
    private CanvasGroup _canvas;
    private Coroutine _changingAlpha;

    private void Awake()
    {
        _canvas = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _active = true;
        if (_changingAlpha != null)
            StopCoroutine(_changingAlpha);
        _changingAlpha = StartCoroutine(ChangeAlpha(1));
    }

    public void Hide()
    {
        _active = false;
        if (_changingAlpha != null)
            StopCoroutine(_changingAlpha);
        _changingAlpha = StartCoroutine(ChangeAlpha(0));
    }

    private IEnumerator ChangeAlpha(float target)
    {
        TrySetInteractions(false);
        var timer = 0f;
        var startAlpha = _canvas.alpha;
        while (timer < _time)
        {
            _canvas.alpha = Mathf.Lerp(startAlpha, target, timer / _time);
            yield return null;
            timer += Time.deltaTime;
        }
        _canvas.alpha = target;
        TrySetInteractions(_active);
    }

    private void TrySetInteractions(bool isON)
    {
        if (_changeInteractions)
        {
            _canvas.interactable = isON;
            _canvas.blocksRaycasts = isON;
        }
    }


}
