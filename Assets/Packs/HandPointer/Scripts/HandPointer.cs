using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public class HandPointer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private HandAnimatorEventListener _animationEvents;

    public event UnityAction<Vector2> MouseDown;
    public event UnityAction<Vector2> MouseUp;

    private void OnEnable()
    {
        _animationEvents.HandPressed += HandlePress;
    }

    private void OnDisable()
    {
        _animationEvents.HandPressed -= HandlePress;
    }

    private void Update()
    {
        HandleMousePosition(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            string animation = HandAnimations.MouseDown;
            _animator.SetTrigger(animation);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            string animation = HandAnimations.MouseUp;
            _animator.SetTrigger(animation);
            MouseUp?.Invoke(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Q))
            _animator.SetTrigger(HandAnimations.Angry);
        if (Input.GetKeyDown(KeyCode.W))
            _animator.SetTrigger(HandAnimations.Ok);
        if (Input.GetKeyDown(KeyCode.E))
            _animator.SetTrigger(HandAnimations.ThumbUp);
        if (Input.GetKeyDown(KeyCode.T))
            _animator.SetTrigger(HandAnimations.MouseDownHit);
    }

    private void HandlePress()
    {
        MouseDown?.Invoke(Input.mousePosition);
    }

    private void HandleMousePosition(Vector2 mousePosition)
    {
        var ray = Camera.main.ScreenPointToRay(mousePosition);
        transform.position = ray.origin + ray.direction * 10;
    }
}
