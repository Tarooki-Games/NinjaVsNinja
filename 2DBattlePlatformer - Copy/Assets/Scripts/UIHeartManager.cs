
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHeartManager : MonoBehaviour
{
    [SerializeField] Player[] _players;
    
    [SerializeField] Image[] _faceImages;
    [SerializeField] Image[] _p1HeartImages;
    [SerializeField] Image[] _p2HeartImages;

    [SerializeField] Sprite _emptySprite;
    [SerializeField] Sprite _fullSprite;
    
    [SerializeField] Sprite _p1DeadFaceSprite;
    [SerializeField] Sprite _p2DeadFaceSprite;
    
    void Awake()
    {
        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].OnHeartLost += UIHeartManagerOnHeartLost;
            _players[i].OnHeartRecovered += UIHeartManagerOnHeartRecovered;
        }
    }

    void Start()
    {
        _emptySprite = GameAssets.GetInstance()._pfHeartEmpty.GetComponent<Image>().sprite;
        _fullSprite = GameAssets.GetInstance()._pfHeartFull.GetComponent<Image>().sprite;
        _p1DeadFaceSprite = GameAssets.GetInstance()._pfDeadFace1.GetComponent<Image>().sprite;
        _p2DeadFaceSprite = GameAssets.GetInstance()._pfDeadFace2.GetComponent<Image>().sprite;

        BattleManager.GetInstance().OnWinConditionMet += UIHeartManagerOnWinConditionMet;
    }

    private void UIHeartManagerOnWinConditionMet(int winner, int winCon)
    {
        if (winner == 1)
            _faceImages[2].sprite = _p2DeadFaceSprite;
        else if (winner == 2)
            _faceImages[1].sprite = _p1DeadFaceSprite;
    }

    void UIHeartManagerOnHeartLost(int playerNumber, int currentHearts)
    {
        if (playerNumber == 1)
            _p1HeartImages[currentHearts].sprite = _emptySprite;
        else if (playerNumber == 2)
            _p2HeartImages[currentHearts].sprite = _emptySprite;
        CheckForDeath(playerNumber, currentHearts);
    }

    void CheckForDeath(int playerNumber, int currentHearts)
    {
        if (currentHearts <= 0)
        {
            if (playerNumber == 1)
                _faceImages[1].sprite = _p1DeadFaceSprite;
            else if (playerNumber == 2)
                _faceImages[2].sprite = _p2DeadFaceSprite;
        }
    }

    void UIHeartManagerOnHeartRecovered(int playerNumber, int currentHearts)
    {
        if (playerNumber == 1)
            _p1HeartImages[currentHearts - 1].sprite = _fullSprite;
        else if (playerNumber == 2)
            _p2HeartImages[currentHearts - 1].sprite = _fullSprite;
    }

    private void OnDisable()
    {
        
        for (int i = 1; i < _players.Length; i++)
        {
            _players[i].OnHeartLost -= UIHeartManagerOnHeartLost;
            _players[i].OnHeartRecovered -= UIHeartManagerOnHeartRecovered;
        }
        BattleManager.GetInstance().OnWinConditionMet -= UIHeartManagerOnWinConditionMet;
    }
}
