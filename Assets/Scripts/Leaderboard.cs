#pragma warning disable

using System.Collections;
using Agava.YandexGames;
using Agava.YandexGames.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private string _name = "PlaytestBoard";
    [SerializeField] private EntryView _playerInfoTemplate;
    [SerializeField] private EntryView _playerEntryView;
    [SerializeField] private Transform _container;
    [SerializeField] private ScreenAppear _leaderboardWindow;
    [SerializeField] private ScreenAppear _loginWindow;
    [SerializeField] private EntryViewPool _playerEntriesViewPool;

    private List<EntryView> _entryViews = new List<EntryView>();
    private string _playerName;

    public string Name => _name;

    private IEnumerator Start()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        yield break;
#endif
        YandexGamesSdk.CallbackLogging = true;

        yield return YandexGamesSdk.WaitForInitialization();
    }

    private void Update()
    {
        AudioListener.pause = WebApplication.InBackground;
    }

    public void TryOpenLeaderboard()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return;
#endif  
        Agava.YandexGames.Leaderboard.GetEntries(_name, (result) =>
        {
            foreach (var entryView in _entryViews)
                entryView.gameObject.SetActive(false);

            _entryViews.Clear();

            Agava.YandexGames.Leaderboard.GetPlayerEntry(_name, (playerEntry) =>
                _playerEntryView.Init(playerEntry.rank.ToString(), playerEntry.player.publicName, playerEntry.score.ToString()));

            foreach (var entry in result.entries)
            {
                EntryView entryView = _playerEntriesViewPool.GetFreeObject();
                entryView.Init(entry.rank.ToString(), entry.player.publicName, entry.score.ToString());
                entryView.gameObject.SetActive(true);
                _entryViews.Add(entryView);
            }
            _leaderboardWindow.Appear();
        },
        (error) => 
        {
            _loginWindow.Appear();
        });
    }

    public void OnRequestDataButtonDown()
    {
        PlayerAccount.RequestPersonalProfileDataPermission();
    }

    public void OnAuthorizeButtonDown()
    {
        PlayerAccount.Authorize();
    }
}
