using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriesView : MonoBehaviour
{
    [SerializeField] private Tries _counter;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _counter.TriesChanged += OnCountChanged;
    }

    private void OnDisable()
    {
        _counter.TriesChanged -= OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        _text.text = count.ToString();
    }
}
