using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BuyTriesButton : MonoBehaviour
{
    [SerializeField] Tries _tries;
    [SerializeField] TMP_Text _text;

    private void Awake()
    {
        _text.text = "+" + _tries.AdBuyAmount.ToString();
    }
}
