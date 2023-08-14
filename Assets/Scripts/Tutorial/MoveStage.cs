using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStage : TutorialStage
{
    [SerializeField] private SpriteColorAnimation _landmark;
    [SerializeField] private Net _net;
    [SerializeField] private Aviaries _aviaries;
    [SerializeField] private Transform _searchPoint;
    [SerializeField] private float _startDelay;


    private void OnEnable()
    {
        _aviaries.GoodAction += OnMove;
    }

    private void OnDisable()
    {
        _aviaries.GoodAction -= OnMove;
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
        var aviaries = FindObjectsOfType<Aviary>();
        var nearest = aviaries[0];
        foreach (var aviary in aviaries)
        {
            var nearestDistanse = Vector3.Distance(nearest.transform.position, _searchPoint.position);
            var aviaryDistanse = Vector3.Distance(aviary.transform.position, _searchPoint.position);
            if (nearestDistanse > aviaryDistanse)
            {
                nearest = aviary;
            }
        }
        _landmark.Show();
    }

    private void OnMove()
    {
        Complete();
        _landmark.Hide();
    }
}
