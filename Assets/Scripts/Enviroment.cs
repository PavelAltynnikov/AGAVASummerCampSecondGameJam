using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [SerializeField] private Game _game;

    private GameObject _current;
    private LevelType _type;

    private void OnEnable()
    {
        _game.LevelStarted += TryChange;
    }

    private void OnDisable()
    {
        _game.LevelStarted -= TryChange;
    }

    private void TryChange(int level, LevelType type)
    {
        if (type != _type)
        {
            Change(type);
        }
    }

    private void Change(LevelType type)
    {
        Destroy(_current);
        _current = Instantiate(type.Enviroment, transform);
    }
}
