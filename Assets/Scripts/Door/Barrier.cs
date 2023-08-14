using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class Barrier : Door
{
    [SerializeField] private NavMeshObstacle _obstacle;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void Open()
    {
        _animator.SetBool("Open", true);
    }

    public override void Close()
    {
        _animator.SetBool("Open", false);
    }

    private void OnClosed()
    {
        _obstacle.enabled = true;
    }
}
