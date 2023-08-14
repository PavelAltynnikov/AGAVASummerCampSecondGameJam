using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCycleFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private RectTransform _ui;

    private Transform _target;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_target)
            UpdatePosition();
    }

    public void Init(Transform target)
    {
        _target = target;
    }

    private void UpdatePosition()
    {
        _rectTransform.anchoredPosition = WorldToCanvasPoint();
    }

    private Vector2 WorldToCanvasPoint()
    {
        var camera = Camera.main;
        var screenPosition = camera.WorldToScreenPoint(_target.position);
        var point = new Vector2();
        point.x = (screenPosition.x / camera.pixelWidth - 0.5f) * _ui.rect.width;
        point.y = (screenPosition.y / camera.pixelHeight - 0.5f) * _ui.rect.height;
        return point;
    }
}
