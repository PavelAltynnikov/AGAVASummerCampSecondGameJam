using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RSG;
using TMPro;

public class LevelDoneScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private PopupText _levetText;
    [SerializeField] private PopupText _title;
    [SerializeField] private PopupText _scorePopup;
    [SerializeField] private ComboText _score;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private PopupText _button;
    [SerializeField] private CanvasGroup _canvas;

    private IPromiseTimer _timer = new PromiseTimer();
    private Color _backColor;

    public void Appear(int score, int level)
    {
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;

        Color startColor = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _background.color = startColor;

        float fadeDuration = 1f;
        _timer.WaitWhile(time =>
        {
            _background.color = Color.Lerp(startColor, _backColor, time.elapsedTime / fadeDuration);
            return time.elapsedTime < fadeDuration;
        });

        float delay = 0.2f;
        _levetText.Show();
        _timer.WaitFor(delay).Then(() =>
        {
            _title.Show();
            _timer.WaitFor(delay).Then(() =>
            {
                _scorePopup.Show("+" + score);
                _timer.WaitFor(delay * 2).Then(() =>
                {
                    _button.Show();
                });
            });
        });
    }

    private void OnEnable()
    {
        _score.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        _score.ScoreChanged -= OnScoreChanged;
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

    private void OnScoreChanged(int score)
    {
        _scoreText.text = "+" + score.ToString();
    }
}
