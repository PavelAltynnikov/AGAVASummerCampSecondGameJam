using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSelectSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Net _net;

    private void OnEnable()
    {
        _net.Selected += Play;
    }

    private void OnDisable()
    {
        _net.Selected -= Play;
    }

    private void Play()
    {
        _audio.Play();
    }
}
