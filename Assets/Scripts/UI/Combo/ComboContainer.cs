using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboContainer : MonoBehaviour
{
    private int _comboValue;

    public event UnityAction<int, Vector3> Combined;
    public event UnityAction<Vector3> LoosedStreak;

    private void Start()
    {
        _comboValue = 0;
    }

    public void AddStreak(Vector3 worldPosition)
    {
        _comboValue++;

        if (_comboValue > 1)
            Combined?.Invoke(_comboValue, worldPosition);
    }

    public void ResetStreak(Vector3 worldPosition)
    {
        _comboValue = 0;

        LoosedStreak?.Invoke(worldPosition);
    }
}
