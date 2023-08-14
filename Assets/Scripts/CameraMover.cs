using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Transform _gamePosition;
    [SerializeField] private Transform _startPosition;

    private void Awake()
    {
        transform.position = _startPosition.position;
        transform.rotation = _startPosition.rotation;
    }

    private void Start()
    {
        StartCoroutine(Move(_gamePosition, 2f, 1));
    }

    private IEnumerator Move(Transform targetTransform, float duration, float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float time = 0;
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        while (time < duration)
        {
            float value = Ease.EaseInEaseOut(time / duration);

            transform.position = Vector3.Lerp(position, targetTransform.position, value);
            transform.rotation = Quaternion.Lerp(rotation, targetTransform.rotation, value);
            yield return null;
            time += Time.deltaTime;
        }

        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
