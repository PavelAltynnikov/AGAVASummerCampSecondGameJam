using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TapMark : MonoBehaviour
{
    private Vector3 _initialScale;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _initialScale = transform.localScale;
        _sprite = GetComponent<SpriteRenderer>();
        _sprite.color = new Color(1, 1, 1, 0);
    }

    public void Show()
    {
        StartCoroutine(AppearLoop());
    }

    public void Hide()
    {
        StartCoroutine(DisppearLoop());
    }

    private IEnumerator AppearLoop()
    {
        float duration = 0.1f;
        float time = 0;
        Vector3 scale = _initialScale * 1.1f;
        Vector3 targetScale = _initialScale;
        Color color = new Color(1, 1, 1, 0);
        Color targetColor = new Color(1, 1, 1, 1);
        while (time < duration)
        {
            float value = Ease.EaseInEaseOut(time / duration);
            transform.localScale = Vector3.Lerp(scale, targetScale, value);
            _sprite.color = Color.Lerp(color, targetColor, value);
            yield return null;
            time += Time.deltaTime;
        }

        transform.localScale = _initialScale;
        _sprite.color = targetColor;
    }

    private IEnumerator DisppearLoop()
    {
        float duration = 0.1f;
        float time = 0;
        Vector3 scale = _initialScale;
        Vector3 targetScale = _initialScale * 1.1f;
        Color color = new Color(1, 1, 1, 1);
        Color targetColor = new Color(1, 1, 1, 0);
        while (time < duration)
        {
            float value = Ease.EaseInEaseOut(time / duration);
            transform.localScale = Vector3.Lerp(scale, targetScale, value);
            _sprite.color = Color.Lerp(color, targetColor, value);
            yield return null;
            time += Time.deltaTime;
        }

        transform.localScale = _initialScale;
        _sprite.color = targetColor;
    }
}
