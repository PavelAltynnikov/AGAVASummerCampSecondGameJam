using UnityEngine;
using System;
using System.Collections;

public class CameraFlight : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [Header("FlightKeyPoints")]
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform finalTransform;

    [Header("FlightParameters")]
    [SerializeField] private float flightSpeed;

    private Action whenCameraOnPosition;

    private void OnEnable() {
        whenCameraOnPosition += ReturnPlayability;
    }

    private void OnDisable() {
        whenCameraOnPosition -= ReturnPlayability;
    }

    private void Start() 
    {
        BlockPlayability();

        Transform cameraTransform = camera.transform;
        CopyTransform(ref cameraTransform, startTransform);
        
        StartCoroutine("Flight");
    }

    private void CopyTransform(ref Transform source, Transform target) 
    {
        source.position = target.position;
        source.rotation = target.rotation;
        source.localScale = target.localScale;
    }

    private IEnumerator Flight() 
    {
        //Нужно добавить скрытие интерфейса
        while (Vector3.Distance(camera.transform.position, finalTransform.position) > 0.5f) {
            camera.transform.position = Vector3.Lerp(camera.transform.position,finalTransform.position, flightSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        camera.orthographic = true;
        whenCameraOnPosition?.Invoke();
    }

    private void BlockPlayability() {
        //Тут скрываем интерфейс и забираем возможность играть
    }

    private void ReturnPlayability(){
        //Тут показываем интерфейс и даём возможность играть
    }
}