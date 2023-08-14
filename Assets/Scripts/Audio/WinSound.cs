using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Game _game;
    [SerializeField] private Aviaries _aviaries;

    private bool _levelcompleted;

    private void OnEnable()
    {
        _game.LevelCompleted += Complete;
        _aviaries.GoodAction += TryPlay;
    }

    private void OnDisable()
    {
        _game.LevelCompleted -= Complete;
        _aviaries.GoodAction += TryPlay;
    }

    private void Complete()
    {
        _levelcompleted = true;
    }

    private void TryPlay()
    {
        if (_levelcompleted)
            _audio.Play();
    }


}
