using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardButton : MonoBehaviour
{
    [SerializeField] private Leaderboard _leaderboard;
    [SerializeField] private Leaderboard _leaderboardWithWarning;

    public void OnLeaderboardButtonDown()
    {
        if (PlayerAccount.HasPersonalProfileDataPermission)
           _leaderboard.TryOpenLeaderboard();
        else
            _leaderboardWithWarning.TryOpenLeaderboard();
    }
}
