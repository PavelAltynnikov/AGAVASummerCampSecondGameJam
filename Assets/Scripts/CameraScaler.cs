using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraScaler : MonoBehaviour
{
    [SerializeField] private float _targetWidth = 10;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (_camera.aspect > 1)
            this.enabled = false;
    }

    private void FixedUpdate()
    {
        float ratio = (float)Screen.height / (float)Screen.width;
        _camera.orthographicSize = _targetWidth * ratio;
    }
}
