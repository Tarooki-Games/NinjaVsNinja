using System;
using TMPro;
using UnityEngine;

public class UI_CoinsCollected : MonoBehaviour
{
    TMP_Text _text;
    [SerializeField] int _playerNumber;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // Causes Garbage Allocation
        // _text.SetText($"Coins: {Coin.CoinsCollected}");

        // Does not cause Garbage Allocation
        _text.SetText(Coin.CoinsCollected.ToString());
    }
}
