using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartPopup : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private PopupText _popup;

    private void OnEnable()
    {
        _game.LevelStarted += Show;
    }

    private void OnDisable()
    {
        _game.LevelStarted -= Show;
    }

    private void Show(int level, LevelType type)
    {
        var text = "LEVEL " + level.ToString();
        _popup.Show(text);
    }
}
