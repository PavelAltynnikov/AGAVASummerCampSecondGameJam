using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboPresenter : MonoBehaviour
{
    [SerializeField] private ComboContainer _container;
    [SerializeField] private ComboEffect[] _niceTemplates;
    [SerializeField] private ComboEffect[] _badTemplates;

    private void OnEnable()
    {
        _container.Combined += OnCombined;
        _container.LoosedStreak += OnLoosedStreak;
    }

    private void OnDisable()
    {
        _container.Combined -= OnCombined;
        _container.LoosedStreak -= OnLoosedStreak;
    }

    private void OnCombined(int value, Vector3 worldPosition)
    {
        var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        var template = _niceTemplates[Random.Range(0, _niceTemplates.Length)];
        Instantiate(template, screenPosition + Vector3.up * 100, Quaternion.identity, transform);
    }

    private void OnLoosedStreak(Vector3 worldPosition)
    {
        var screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        var template = _badTemplates[Random.Range(0, _badTemplates.Length)];
        Instantiate(template, screenPosition + Vector3.up * 50, Quaternion.identity, transform);
    }
}
