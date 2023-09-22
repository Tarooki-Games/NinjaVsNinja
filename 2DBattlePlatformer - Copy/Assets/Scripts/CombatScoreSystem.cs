using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class CombatScoreSystem : MonoBehaviour
{
    int _player1Points;
    int _player2Points;

    static int _highScore;
    
    static CombatScoreSystem _instance;

    public static CombatScoreSystem GetInstance() => _instance;
    
    [SerializeField] TMP_Text[] _texts;

    public int Player1Points { get => _instance._player1Points; set => _instance._player1Points = value; }
    public int Player2Points { get => _instance._player2Points; set => _instance._player2Points = value; }

    public event Action<int> OnCoinVictory;
    
    public void ResetHighScore()
    {
        _highScore = 0;
        PlayerPrefs.SetInt("HighScore", _highScore);
        PlayerPrefs.SetInt("Player1Score", 0);
        PlayerPrefs.SetInt("Player2Score", 0);
        UpdateScoreTextUI();
    }

    void OnEnable()
    {
        // Get HighScore before setting the text.
        _highScore = PlayerPrefs.GetInt("HighScore");
        UpdateScoreTextUI();
    }

    void Awake()
    {
        ResetHighScore();
        _instance = this;
        //_texts = GetComponentsInChildren<TMP_Text>();
    }

    void Update()
    {
        if (Input.GetButton("ResetHighScore"))
        {
            ResetHighScore();
        }
    }

    void UpdateScoreTextUI()
    {
        _texts[0].SetText(_highScore.ToString());
        _texts[1].SetText(PlayerPrefs.GetInt("Player1Score").ToString());
        _texts[2].SetText(PlayerPrefs.GetInt("Player2Score").ToString());
    }

    public static void UpdatePoints(int points, int playerNumber)
    {
        _instance.AddThenSetText(points, playerNumber);
    }

    void AddThenSetText(int points, int playerNumber)
    {
        if (playerNumber == 1)
        {
            _player1Points += points;
            PlayerPrefs.SetInt("Player1Score", _player1Points);
            if (_player1Points >= _highScore)
            {
                _highScore = _player1Points;
                PlayerPrefs.SetInt("HighScore", _highScore);
            }
        }
        else if (playerNumber == 2)
        {
            _player2Points += points;
            PlayerPrefs.SetInt("Player2Score", _player2Points);
            if (_player2Points >= _highScore)
            {
                _highScore = _player2Points;
                PlayerPrefs.SetInt("HighScore", _highScore);
            }
        }

        UpdateScoreTextUI();

        if (points > 0 && _highScore >= 10)
            OnCoinVictory?.Invoke(playerNumber);
    }
}
