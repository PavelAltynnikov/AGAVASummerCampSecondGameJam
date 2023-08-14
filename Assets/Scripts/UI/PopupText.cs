using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float _appearTime = 2;
    [SerializeField] private string[] _content;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _appearPower = 2;
    [SerializeField] private float _delay = 0;

    private Damping _dumping;
    private Coroutine _appearTask;
    private Coroutine _disappearTask;
    private Coroutine _disappearDelayTask;
    private Camera _camera;

    private void Awake()
    {
        ResetLocalScale();
        _camera = Camera.main;
        _dumping = new Damping(0.5f, 3, 0, _appearPower);
    }

    private void ResetLocalScale()
    {
        _transform.localScale = Vector3.zero;
    }

    public void Show(string text = "")
    {
        ResetLocalScale();
        if (text == "")
        {
            if (_content.Length > 0)
                _text.text = _content[Random.Range(0, _content.Length)];
        }
        else
            _text.text = text;

        if (_appearTask != null)
            StopCoroutine(_appearTask);

        if (_disappearTask != null)
            StopCoroutine(_disappearTask);

        if (_disappearDelayTask != null)
            StopCoroutine(_disappearDelayTask);

        _appearTask = StartCoroutine(AppearWithDelay());
        _disappearDelayTask = StartCoroutine(DisappearAfter());
    }

    public void Show(Vector3 position, string text = "")
    {
        _transform.position = _camera.WorldToScreenPoint(position);
        Show(text);
    }

    private IEnumerator DisappearAfter()
    {
        yield return new WaitForSeconds(_appearTime);
        if (_appearTask != null)
            StopCoroutine(_appearTask);

        _disappearTask = StartCoroutine(Disappear());
    }

    private IEnumerator AppearWithDelay()
    {
        yield return new WaitForSeconds(_delay);
        _transform.localScale = Vector3.zero;

        float duration = 0.5f;
        float time = 0;
        while (time < duration)
        {
            float scale = GetAppearValue(time / duration);
            _transform.localScale = Vector3.one * scale;
            yield return null;
            time += Time.deltaTime;
        }

        _transform.localScale = Vector3.one;
    }

    private IEnumerator Disappear()
    {
        _transform.localScale = Vector3.one;

        float duration = 0.2f;
        float time = 0;
        while (time < duration)
        {
            float scale = 1 - time / duration;
            _transform.localScale = Vector3.one * scale;

            yield return null;
            time += Time.deltaTime;

        }

        _transform.localScale = Vector3.zero;
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
}
