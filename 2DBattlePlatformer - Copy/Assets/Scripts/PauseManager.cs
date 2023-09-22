using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    void Awake()
    {
        _panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bool isBattling = BattleManager.GetInstance().IsBattling;
            
            // Toggle BattleManager.IsBattling and pause panel game object
            BattleManager.GetInstance().IsBattling = !isBattling;
            _panel.SetActive(!_panel.activeSelf);
            
            Time.timeScale = isBattling ? 0 : 1;
        }
    }
}
