using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionFeedbackSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioTry;
    [SerializeField] private AudioSource _audioGood;
    [SerializeField] private AudioSource _audioBad;
    [SerializeField] private Aviaries _aviaries;
    [SerializeField] private Net _net;

    private void OnEnable()
    {
        _aviaries.GoodAction += PlayGood;
        _aviaries.BadAction += PlayBad;
        _net.BadTry += PlayTry;
    }

    private void OnDisable()
    {
        _aviaries.GoodAction -= PlayGood;
        _aviaries.BadAction -= PlayBad;
        _net.BadTry -= PlayTry;
    }

    private void PlayGood()
    {
        _audioGood.Play();
    }

    private void PlayBad()
    {
        _audioBad.Play();
    }

    private void PlayTry()
    {
        _audioTry.Play();
    }
}
