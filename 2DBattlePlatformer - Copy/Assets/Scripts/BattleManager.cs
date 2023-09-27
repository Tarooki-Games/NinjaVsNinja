using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    static BattleManager _instance;
    public static BattleManager GetInstance()
    {
        return _instance;
    }

    [SerializeField] bool _isBattling;
    public bool IsBattling
    {
        get { return _isBattling; }   // get method
        set { _isBattling = value; }  // set method
    }
    
    List<Timer> _timers;
    [SerializeField] int _roundTime = 90;
    [SerializeField] int _coinsToWin = 10;
    [SerializeField] float _spawnInterval = 3.6f;

    [SerializeField] GameObject _playersGO;
    [SerializeField] List<Player> _players;

    private AnimationEvents _animationEvents;
    private CombatScoreSystem _combatScoreSystem;
    
    public event Action<int, int> OnWinConditionMet; 
    
    void Awake()
    {
        _animationEvents = GetComponentInChildren<AnimationEvents>();
        _combatScoreSystem = GetComponentInChildren<CombatScoreSystem>();
        
        _instance = this;
        
        _timers = GetComponentsInChildren<Timer>().ToList();
        _players = _playersGO.GetComponentsInChildren<Player>().ToList();

        ResetScores();
    }

    void OnEnable()
    {
        foreach (Timer t in _timers)
        {
            t.Duration = _roundTime;
            if (t._mainTimer)
            {
                t.OnRoundTimeUp += BattleManagerOnRoundTimeUp;
                t.IntervalTime = _spawnInterval;
            }
        }

        _combatScoreSystem.CoinsToWin = _coinsToWin;
        _combatScoreSystem.OnCoinVictory += BattleManagerOnCoinVictory;

        for (int i = 0; i < _players.Count; i++)
            _players[i].OnPlayerDeath += BattleManagerOnPlayerDeath;
        
        _animationEvents.OnFightCalledInAnim += BattleManagerOnFightCalledInAnim;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("P1Score: " + PlayerPrefs.GetInt("Player1Score"));
            Debug.Log("P2Score: " + PlayerPrefs.GetInt("Player2Score"));
            Debug.Log("HiScore: " + PlayerPrefs.GetInt("HighScore"));
        }
    }

    void ResetScores()
    {
        PlayerPrefs.SetInt("Player1Score", 0);
        PlayerPrefs.SetInt("Player2Score", 0);
        PlayerPrefs.SetInt("HighScore", 0);
    }
    

    void BattleManagerOnFightCalledInAnim()
    {
        // Set IsBatttling to True
        _isBattling = true;

        foreach (Timer t in _timers)
        {
            t.Begin(_roundTime);
        }
    }
    
    // ######## //
    // GAME END //
    // ######## //
    
    // WIN CONDITION 1: TIMER VICTORY!
    void BattleManagerOnRoundTimeUp()
    {
        _isBattling = false;
        
        int p1Score = PlayerPrefs.GetInt("Player1Score");
        int p2Score = PlayerPrefs.GetInt("Player2Score");
        
        // Check Count Count for Victory
        if (p1Score == p2Score)
        {
            OnWinConditionMet?.Invoke(0, 1);
            Debug.Log("TIME UP! Round Drawn!");
        }
        else if (p1Score > p2Score)
        {
            OnWinConditionMet?.Invoke(1, 1);
            Debug.Log("TIME UP! P1 Wins Round!");
        }
        else if (p2Score > p1Score)
        {
            OnWinConditionMet?.Invoke(2, 1);
            Debug.Log("TIME UP! P2 Wins Round!");
        }
    }
    
    // WIN CONDITION 2: DEATH VICTORY!
    void BattleManagerOnPlayerDeath(int loser)
    {
        _isBattling = false;
        
        Debug.Log($"DEATH VICTORY! Player {loser} loses!");
        if (loser == 1)
        {
            OnWinConditionMet?.Invoke(2, 2);
            Debug.Log($"DEATH VICTORY! Player 2 Wins!");
        }
        else
        {
            OnWinConditionMet?.Invoke(1, 2);
            Debug.Log($"DEATH VICTORY! Player 1 Wins!");
        }
    }
    
     // WIN CONDITION 3: COIN VICTORY!
    void BattleManagerOnCoinVictory(int winner)
    {
        _isBattling = false;
        OnWinConditionMet?.Invoke(winner, 3);
        Debug.Log($"COIN VICTORY! Player {winner} Wins!");
    }

    void OnDisable()
    {
        _animationEvents.OnFightCalledInAnim -= BattleManagerOnFightCalledInAnim;
        
        foreach (Timer t in _timers)
        {
            if (t._mainTimer)
                t.OnRoundTimeUp -= BattleManagerOnRoundTimeUp;
        }
        
        _combatScoreSystem.OnCoinVictory -= BattleManagerOnCoinVictory;
        
        for (int i = 0; i < _players.Count; i++)
            _players[i].OnPlayerDeath -= BattleManagerOnPlayerDeath;
    }
}
