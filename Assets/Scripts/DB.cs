using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB
{
    private static readonly string LevelKey = "level";
    private static readonly string ScoreKey = "score";
    private static readonly string InGameSoundKey = "ingamesound";

    public static int GetLevel() => PlayerPrefs.GetInt(LevelKey) + 1;

    public static bool GetInGameSound() => PlayerPrefs.GetInt(InGameSoundKey) != 0 || PlayerPrefs.HasKey(InGameSoundKey) == false;

    public static void SetInGameSound(bool isOn) => PlayerPrefs.SetInt(InGameSoundKey, isOn? 1:0);

    public static void IncreaseLevel() => PlayerPrefs.SetInt(LevelKey, PlayerPrefs.GetInt(LevelKey) + 1);

    public static void ResetLevel() => PlayerPrefs.SetInt(LevelKey, 0);

    public static int GetScore() => PlayerPrefs.GetInt(ScoreKey) + 1;

    public static void AddScore(int value) => PlayerPrefs.SetInt(ScoreKey, PlayerPrefs.GetInt(ScoreKey) + value);

    public static void ResetScore() => PlayerPrefs.SetInt(ScoreKey, 0);
}
