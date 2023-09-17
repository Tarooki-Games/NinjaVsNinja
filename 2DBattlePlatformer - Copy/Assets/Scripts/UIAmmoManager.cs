using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmoManager : MonoBehaviour
{
    [SerializeField] ProjectileLauncher[] _projLaunchers;
    [SerializeField] Image[] _ammoImages;
    [SerializeField] TMP_Text[] _ammoTexts;

    [SerializeField] Sprite _shurikenSprite;
    [SerializeField] Sprite _fireballSprite;
    [SerializeField] Sprite _emptySprite;
    
    void Awake()
    {
        for (int i = 1; i < _projLaunchers.Length; i++)
        {
            _projLaunchers[i].OnAmmoCollected += UIAmmoManagerOnAmmoCollected;
            _projLaunchers[i].OnAmmoFired += UIAmmoManagerOnAmmoFired;
        }
    }

    void Start()
    {
        _shurikenSprite = GameAssets.GetInstance()._pfAmmoShuriken.GetComponent<Image>().sprite;
        _fireballSprite = GameAssets.GetInstance()._pfAmmoFireball.GetComponent<Image>().sprite;
        _emptySprite = GameAssets.GetInstance()._pfAmmoEmpty.GetComponent<Image>().sprite;

        for (int i = 1; i < _projLaunchers.Length; i++)
        {
            UIAmmoManagerOnAmmoFired(i, 0);
        }
    }

    void UIAmmoManagerOnAmmoFired(int playerNumber, int currentAmmo)
    {
        if (currentAmmo <= 0)
        {
            var transform = _ammoImages[playerNumber].GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(72, 72);
            _ammoImages[playerNumber].sprite = _emptySprite;
            _ammoImages[playerNumber].color = Color.red;
        }
        _ammoTexts[playerNumber].SetText(currentAmmo.ToString());
    }
    
    void UIAmmoManagerOnAmmoCollected(int playerNumber, bool isShuriken, int currentAmmo)
    {
        var transform = _ammoImages[playerNumber].GetComponent<RectTransform>();

        if (isShuriken)
        {
            if (_ammoImages[playerNumber].sprite != _shurikenSprite)
            {
                transform.sizeDelta = new Vector2(99, 99);
                _ammoImages[playerNumber].sprite = _shurikenSprite;
                _ammoImages[playerNumber].color = Color.white;
            }
        }
        else
        {
            if (_ammoImages[playerNumber].sprite != _fireballSprite)
            {
                transform.sizeDelta = new Vector2(180, 180);
                _ammoImages[playerNumber].sprite = _fireballSprite;
                _ammoImages[playerNumber].color = Color.white;
            }
        }

        _ammoTexts[playerNumber].SetText(currentAmmo.ToString());
    }
}
