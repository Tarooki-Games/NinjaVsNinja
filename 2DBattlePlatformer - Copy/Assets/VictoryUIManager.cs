using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryUIManager : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    [SerializeField] List<TMP_Text> texts;

    void Awake() => _panel.SetActive(false);

    void Start() => BattleManager.GetInstance().OnWinConditionMet += VictoryUIManagerOnWinConditionMet;

    void VictoryUIManagerOnWinConditionMet(int winner, int winCondition)
    {
        _panel.SetActive(true);
        WinConVictoryTextUpdate(winCondition);
        PlayerTextsUpdate(winner);
    }

    void WinConVictoryTextUpdate(int winCon)
    {
        if (winCon == 1)
            texts[0].SetText("TIMER VICTORY!");
        else if (winCon == 2)
            texts[0].SetText("SURVIVAL VICTORY");
        else if (winCon == 3)
            texts[0].SetText("GOLD VICTORY");
    }
    
    void PlayerTextsUpdate(int winner)
    {
        string p1Text = texts[1].text;
        string p2Text = texts[2].text;
        if (winner == 0)
        {
            texts[0].text = "BATTLE DRAWN!";
            for (int i = 1; i < texts.Count; i++)
                texts[i].SetText("PARTICIPANT");
        }
        else if (winner == 1)
        {
            texts[1].SetText("WINNER");
            texts[2].SetText("LOSER");
        }
        else if (winner == 2)
        {
            texts[2].SetText("WINNER");
            texts[1].SetText("LOSER");
        }
    }
}
