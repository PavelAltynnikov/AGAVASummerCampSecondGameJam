using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreakPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _textParent;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _streakText;
    [SerializeField] private ComboContainer _comboContainer;

    private void OnEnable()
    {
        _comboContainer.Combined += OnCombined;
        _comboContainer.LoosedStreak += OnLoosedStreak;
    }

    private void OnDisable()
    {
        _comboContainer.Combined -= OnCombined;
        _comboContainer.LoosedStreak -= OnLoosedStreak;
    }

    private void OnCombined(int value, Vector3 position)
    {
        _streakText.text = "x" + value.ToString();
        _textParent.SetActive(true);


        if (value == 2)
            _animator.SetTrigger("StartStreak");
        else
            _animator.SetTrigger("AddStreak");
    }

    private void OnLoosedStreak(Vector3 worldPosition)
    {
        _animator.SetTrigger("LooseStreak");
    }
}
