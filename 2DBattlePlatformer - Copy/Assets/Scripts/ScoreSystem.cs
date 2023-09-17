using UnityEngine;

public static class ScoreSystem
{
    public static int Score { get; private set; }
    public static int Player1Score { get; private set; }
    public static int Player2Score { get; private set; }
    static int _highScore;

    public static event System.Action<int> OnScoreChanged;
    public static event System.Action OnHighScoreChanged;

    //void Start()
    //{
    //    _highScore = PlayerPrefs.GetInt("HighScore");
    //}

    public static void Add(int points)
    {
        Score += points;

        PlayerPrefs.SetInt("Score", Score);
        OnScoreChanged?.Invoke(Score);
        //Debug.Log($"Score: {Score} - HighScore: {_highScore} - PlayerPrefs.HighScore: { PlayerPrefs.GetInt("HighScore") } ");

        if (Score > _highScore)
        {
            _highScore = Score;
            PlayerPrefs.SetInt("HighScore", _highScore);
            OnHighScoreChanged?.Invoke();
        }
    }

    public static void ResetHighScore()
    {
        _highScore = 0;
        PlayerPrefs.SetInt("HighScore", _highScore);
        OnHighScoreChanged?.Invoke();
    }
}
