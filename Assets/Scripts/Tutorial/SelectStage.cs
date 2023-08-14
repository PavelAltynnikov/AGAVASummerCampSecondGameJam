using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : TutorialStage
{
    [SerializeField] private TutorialCycleFollow _follower;
    [SerializeField] private CanvasGroupPopUp _targeter;
    [SerializeField] private Net _net;
    [SerializeField] private Transform _searchPoint;
    [SerializeField] private float _startDelay;


    private void OnEnable()
    {
        _net.Selected += OnSelection;
    }

    private void OnDisable()
    {
        _net.Selected -= OnSelection;
    }

    protected override void AtStart() 
    {
        Invoke(nameof(TryInit), _startDelay);
    }

    private void TryInit()
    {
        if (Active)
        {
            Init();
        }
    }

    private void Init()
    {
        var animals = FindObjectsOfType<Animal>();
        var nearest = animals[0];
        foreach (var animal in animals)
        {
            var nearestDistanse = Vector3.Distance(nearest.transform.position, _searchPoint.position);
            var animalDistanse = Vector3.Distance(animal.transform.position, _searchPoint.position);
            if (nearestDistanse > animalDistanse)
            {
                nearest = animal;
            }
        }
        _follower.Init(nearest.transform);
        _targeter.Show();
    }

    private void OnSelection()
    {
        Complete();
        _targeter.Hide();
    }
}
