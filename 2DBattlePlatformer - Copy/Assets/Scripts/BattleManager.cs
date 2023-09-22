using System;
using System.Collections;
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

    [SerializeField] GameObject _playersGO;
    [SerializeField] List<Player> _players;
    
    void Awake()
    {
        _instance = this;
        
        _timers = GetComponentsInChildren<Timer>().ToList();
        
        foreach (Timer t in _timers)
        {
            t._duration = _roundTime;
            if (t._mainTimer)
                t.OnRoundTimeUp += BattleManagerOnRoundTimeUp;
        }
        
        CombatScoreSystem.GetInstance().OnCoinVictory += BattleManagerOnCoinVictory;

        _players = _playersGO.GetComponentsInChildren<Player>().ToList();
        for (int i = 0; i < _players.Count; i++)
            _players[i].OnPlayerDeath += BattleManagerOnPlayerDeath;
    }
    
    void OnEnable()
    {
        GetComponentInChildren<AnimationEvents>().OnFightCalledInAnim += BattleManagerOnFightCalledInAnim;
    }

    void BattleManagerOnFightCalledInAnim()
    {
        // Set IsBatttling to True
        IsBattling = true;

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
        IsBattling = false;
        
        int p1Score = PlayerPrefs.GetInt("Player1Score");
        int p2Score = PlayerPrefs.GetInt("Player2Score");
        
        // Check Count Count for Victory
        if (p1Score == p2Score)
        {
            Debug.Log("TIME UP! Round Drawn!");
        }
        else if (p1Score > p2Score)
        {
            Debug.Log("TIME UP! P1 Wins Round!");
        }
        else if (p2Score > p1Score)
        {
            Debug.Log("TIME UP! P2 Wins Round!");
        }
    }
    
    // WIN CONDITION 2: DEATH VICTORY!
     void BattleManagerOnPlayerDeath(int loser)
    {
        IsBattling = false;
        
        Debug.Log($"DEATH VICTORY! Player {loser} loses!");
        if (loser == 1)
            Debug.Log($"DEATH VICTORY! Player 2 Wins!");
        else
            Debug.Log($"DEATH VICTORY! Player 1 Wins!");
    }
    
     // WIN CONDITION 3: COIN VICTORY!
    void BattleManagerOnCoinVictory(int winner)
    {
        IsBattling = false;
        
        Debug.Log($"COIN VICTORY! Player {winner} Wins!");
    }
}
