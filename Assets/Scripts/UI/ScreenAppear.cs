using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenAppear : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private float _fadeDuration = 0.1f;
    [SerializeField] private PopupText[] _popupTexts;
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private InGameInput _input;

    private IPromiseTimer _timer = new PromiseTimer();
    private Color _backColor;

    public void Appear()
    {
        Color startColor = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _background.color = startColor;

        _timer.WaitWhile(time =>
        {
            _background.color = Color.Lerp(startColor, _backColor, time.elapsedTime / _fadeDuration);
            return time.elapsedTime < _fadeDuration;
        });

        foreach (var popupText in _popupTexts)
            popupText.Show();

        _input.IsON = false;
        _canvas.alpha = 1;
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;
    }

    public void Hide()
    {
        StartCoroutine(HideSelf(_fadeDuration));
        _input.IsON = true;
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }

    private IEnumerator HideSelf(float showTime)
    {
        float timer = 0;
        while (timer <= showTime)
        {
            timer += Time.deltaTime;
            _canvas.alpha = Mathf.Lerp(1, 0, timer / showTime);
            yield return null;
        }
    }
    private void Awake()
    {
        _backColor = _background.color;
        _background.color = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }
}
