using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RSG;
public class LoseScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private PopupText _levetText;
    [SerializeField] private PopupText _title;
    [SerializeField] private PopupText _restart;
    [SerializeField] private Tries _tries;
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private Game _game;
    [SerializeField] private InGameInput _input;

    private IPromiseTimer _timer = new PromiseTimer();
    private Color _backColor;
    private int _level;

    public void Appear(int level)
    {
        Color startColor = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _background.color = startColor;

        float fadeDuration = 1f;
        _timer.WaitWhile(time =>
        {
            _background.color = Color.Lerp(startColor, _backColor, time.elapsedTime / fadeDuration);
            return time.elapsedTime < fadeDuration;
        });

        float delay = 0.2f;
        _levetText.Show("Уровень " + level.ToString());
        _timer.WaitFor(delay).Then(() =>
        {
            _title.Show();
            _timer.WaitFor(delay).Then(() =>
            {
                _timer.WaitFor(delay * 2).Then(() =>
                {
                    _restart.Show();
                });
            });
        });
    }

    private void OnEnable()
    {
        _tries.TriesChanged += OnTriesChanged;
        _game.LevelStarted += OnLevelChanged;
        _game.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        _tries.TriesChanged -= OnTriesChanged;
        _game.LevelStarted -= OnLevelChanged;
        _game.LevelCompleted -= OnLevelCompleted;
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

    private void OnTriesChanged(int tries)
    {
        if (tries == 0 && false)
        {
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
            Appear(_level);
            _input.IsON = false;
        }
    }

    private void OnLevelChanged(int level, LevelType type)
    {
        _level = level;
    }

    private void OnLevelCompleted()
    {
        gameObject.SetActive(false);
    }
}
