using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PauseManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    private bool canPause;
    
    void Awake()
    {
        _panel.SetActive(false);
        StartCoroutine(ActivatePause());
        Debug.Log("CanPause = False");
    }

    IEnumerator ActivatePause()
    {
        yield return new WaitForSeconds(5.05f);
        canPause = true;
        Debug.Log("CanPause = True");
    }

    void Update()
    {
        if (canPause && Input.GetButtonDown("Pause"))
        {
            bool isBattling = BattleManager.GetInstance().IsBattling;
            
            // Toggle BattleManager.IsBattling and pause panel game object
            BattleManager.GetInstance().IsBattling = !isBattling;
            _panel.SetActive(!_panel.activeSelf);
            
            Time.timeScale = isBattling ? 0 : 1;
        }
    }
}
