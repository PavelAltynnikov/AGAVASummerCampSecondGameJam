using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ComboText : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private RectTransform _numberPanel;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private float _maxDelay = 1.5f;
    [SerializeField] private int _minValue = 1;
    [SerializeField] private float _scaleDelta = 0.02f;
    [SerializeField] private float _maxScale = 1.1f;

    private Damping _dumping = new Damping(0.5f, 3, 0, 1);
    private RectTransform _labelTransform;

    private int _score = 0;
    private float _numberScale = 1;
    private Coroutine _pulseTask;
    private Coroutine _disappearTask;
    private Coroutine _disappearDelayTask;
    private bool _isActive = false;
    private bool _locked = false;

    public int Value => _score;

    public event UnityAction<int> ScoreChanged;
    public event UnityAction<int> WillDisappear;

    private void Awake()
    {
        _labelTransform = _label.GetComponent<RectTransform>();

        if (_score < _minValue)
        {
            _labelTransform.localScale = Vector3.zero;
            _numberPanel.localScale = Vector3.zero;
        }
    }

    public void Increase(int value = 1)
    {
        if (_locked)
            return;

        _score += value;
        ScoreChanged?.Invoke(_score);
        _number.text = _score.ToString();
        _numberPanel.gameObject.SetActive(_score > _minValue);

        if (_disappearDelayTask != null)
            StopCoroutine(_disappearDelayTask);

        _disappearDelayTask = StartCoroutine(DisappearAfter(_maxDelay));

        if (_isActive)
        {
            StopDesappearing();
            if (_pulseTask != null)
                StopCoroutine(_pulseTask);

            _pulseTask = StartCoroutine(Pulse());
        }
        else
        {
            _isActive = true;
            StartCoroutine(Appear());
        }
    }

    public void QuickReset()
    {
        _score = 0;
        _number.text = _score.ToString();
        _numberPanel.gameObject.SetActive(_score > _minValue);
    }

    public void Reset()
    {
        if (_locked)
            return;

        _locked = true;
        StopAllCoroutines();
        _disappearTask = StartCoroutine(Disappear());
        StartCoroutine(Unlock());
    }

    private IEnumerator Unlock()
    {
        yield return new WaitForSeconds(1);
        _locked = false;
    }

    private IEnumerator DisappearAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _disappearTask = StartCoroutine(Disappear());
    }

    private void StopDesappearing()
    {
        if (_disappearTask != null)
        {
            StopCoroutine(_disappearTask);

            _labelTransform.localScale = Vector3.one;
            _numberPanel.localScale = Vector3.one * _numberScale;
        }
    }

    public IEnumerator Appear()
    {
        _labelTransform.localScale = Vector3.one;
        _numberPanel.localScale = Vector3.one;

        float duration = 0.8f;
        float time = 0;
        while (time < duration)
        {
            float scale = GetAppearValue(time / duration);
            _panel.localScale = Vector3.one * scale;
            yield return null;
            time += Time.deltaTime;
        }
        _panel.localScale = Vector3.one;
    }

    private IEnumerator Disappear()
    {
        WillDisappear?.Invoke(_score);

        float delay = 0.06f;
        float duration = 0.2f;
        float time = 0;
        while (time < duration)
        {
            float labelScale = 1 - time / (duration - delay);
            float numberScale = 1 - (time - delay) / duration;

            if (time >= delay)
                _numberPanel.localScale = Vector3.one * _numberScale * numberScale;

            if (time <= duration - delay)
                _labelTransform.localScale = Vector3.one * labelScale;

            yield return null;
            time += Time.deltaTime;
        }

        _labelTransform.localScale = Vector3.zero;
        _numberPanel.localScale = Vector3.zero;

        _isActive = false;
        _numberScale = 1;
        _score = 0;
    }

    private IEnumerator Pulse()
    {
        _labelTransform.localScale = Vector3.one;
        _numberPanel.localScale = Vector3.one * _numberScale;

        _numberScale += _scaleDelta;

        if (_numberScale > _maxScale)
            _numberScale = _maxScale;

        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            float scale = GetPulseValue(time / duration) * _numberScale;
            _numberPanel.localScale = Vector3.one * scale;
            yield return null;
            time += Time.deltaTime;
        }

        _numberPanel.localScale = Vector3.one * _numberScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            Increase();
    }

    private float GetAppearValue(float percent)
    {
        float linearPart = 0.1f;
        if (percent > linearPart)
        {
            return 1 + _dumping.GetValue((percent - linearPart) / (1 - linearPart));
        }
        else
        {
            return percent / linearPart;
        }
    }

    private float GetPulseValue(float percent)
    {
        return 1 + _dumping.GetValue(percent);
    }
}
