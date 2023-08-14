using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AviaryDoor : Door
{
    [SerializeField] private GameObject _leftPart;
    [SerializeField] private GameObject _rightPart;
    [SerializeField] private float _openDuration = 0.2f;
    [SerializeField] private float _closeDuration = 0.4f;
    [SerializeField] private NavMeshObstacle _obstacle;

    private Coroutine _task;

    public override void Open()
    {
        _obstacle.enabled = false;
        if (_task != null)
            StopCoroutine(_task);

        _task = StartCoroutine(OpenLoop());
    }

    public override void Close()
    {
        if (_task != null)
            StopCoroutine(_task);

        _task = StartCoroutine(CloseLoop());
    }

    private IEnumerator OpenLoop()
    {
        float time = 0;
        Quaternion startLeftRotation = _leftPart.transform.localRotation;
        Quaternion leftPartTargetRotation = Quaternion.Euler(-90, 0, 300);

        Quaternion startRightRotation = _rightPart.transform.localRotation;
        Quaternion rightPartTargetRotation = Quaternion.Euler(-90, 0, -120);
        while (time < _openDuration)
        {
            float value = Ease.EaseInEaseOut(time / _openDuration);
            _leftPart.transform.localRotation = Quaternion.Lerp(startLeftRotation, leftPartTargetRotation, value);
            _rightPart.transform.localRotation = Quaternion.Lerp(startRightRotation, rightPartTargetRotation, value);
            yield return null;
            time += Time.deltaTime;
        }
        _leftPart.transform.localRotation = leftPartTargetRotation;
        _rightPart.transform.localRotation = rightPartTargetRotation;
    }

    private IEnumerator CloseLoop()
    {
        float time = 0;
        Quaternion rightPartRotation = _rightPart.transform.localRotation;
        Quaternion leftPartRotation = _leftPart.transform.localRotation;
        Quaternion rightPartTargetRotation = Quaternion.Euler(-90, 0, 0);
        Quaternion leftPartTargetRotation = Quaternion.Euler(-90, 0, 180);
        while (time < _openDuration)
        {
            float value = Ease.EaseInEaseOut(time / _openDuration);
            _leftPart.transform.localRotation = Quaternion.Lerp(leftPartRotation, leftPartTargetRotation, value);
            _rightPart.transform.localRotation = Quaternion.Lerp(rightPartRotation, rightPartTargetRotation, value);
            yield return null;
            time += Time.deltaTime;
        }
        _leftPart.transform.localRotation = leftPartTargetRotation;
        _rightPart.transform.localRotation = rightPartTargetRotation;

        _obstacle.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            Open();
        else if (Input.GetKeyDown(KeyCode.D))
            Close();
    }
}
