using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Lean.Localization;

public class LocalizationChanger : MonoBehaviour
{
    [SerializeField] private LeanLocalization _leanLocalization;

#if !UNITY_EDITOR && UNITY_WEBGL
    private void Start()
    {
        switch (YandexGamesSdk.Environment.i18n.tld)
        {
            case "com":
                _leanLocalization.CurrentLanguage = "English";
                break;
            case "com.tr":
                _leanLocalization.CurrentLanguage = "Turkish";
                break;
            case "ru":
                _leanLocalization.CurrentLanguage = "Russian";
                break;
            default:
                _leanLocalization.CurrentLanguage = "English";
                Debug.Log("Unknown domain");
                break;
        }
    }
#endif
}