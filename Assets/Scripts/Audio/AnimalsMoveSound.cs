using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimalsMoveSound : MonoBehaviour
{
    [SerializeField] AudioSource _audio;
    [SerializeField] private Net _net;
    [SerializeField] private Aviaries _aviaries;

    private void OnEnable()
    {
        _net.AnimalsMoving += StartPlaying;
        _aviaries.Interacted += StopPlaying;
    }

    private void OnDisable()
    {
        _net.AnimalsMoving -= StartPlaying;
        _aviaries.Interacted -= StopPlaying;
    }

    private void StartPlaying()
    {
        _audio.Play();
    }

    private void StopPlaying()
    {
        _audio.Stop();
    }

}
