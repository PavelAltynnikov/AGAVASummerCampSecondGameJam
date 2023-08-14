using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Aviaries : MonoBehaviour
{
    [SerializeField] private ComboContainer _comboContainer;
    [SerializeField] private Aviary[] _aviaries;

    public event UnityAction<List<Animal>> ReleasedAnimals;
    public event UnityAction Interacted;
    public event UnityAction GoodAction;
    public event UnityAction BadAction;

    private void OnEnable()
    {
        foreach (var item in _aviaries)
        {
            item.ReleasedAnimals += OnReleasedAnimals;
            item.Interacted += OnAviaryInteracted;
            item.BadMove += OnBadAction;
            item.NiceMove += OnGoodAction;
            item.VeryNiceMove += OnGoodAction;
        }
    }

    private void OnDisable()
    {
        foreach (var item in _aviaries)
        {
            item.ReleasedAnimals -= OnReleasedAnimals;
            item.Interacted -= OnAviaryInteracted;
            item.BadMove -= BadAction;
            item.NiceMove -= GoodAction;
            item.VeryNiceMove -= GoodAction;
        }
    }

    private void Start()
    {
        foreach (var item in _aviaries)
            item.Init(_comboContainer);
    }

    private void OnReleasedAnimals(List<Animal> animals)
    {
        ReleasedAnimals?.Invoke(animals);
    }

    private void OnAviaryInteracted(Aviary aviary)
    {
        Interacted?.Invoke();
    }

    private void OnBadAction()
    {
        BadAction?.Invoke();
    }

    private void OnGoodAction()
    {
        GoodAction?.Invoke();
    }
}
