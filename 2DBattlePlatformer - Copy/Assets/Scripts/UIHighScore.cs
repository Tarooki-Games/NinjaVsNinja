using TMPro;
using UnityEngine;

public class UIHighScore : MonoBehaviour
{
    [SerializeField] TMP_Text _highScoreText;

    void Start()
    {
        _highScoreText = GetComponent<TMP_Text>();

        UpdateHighScore();

        ScoreSystem.OnHighScoreChanged += UpdateHighScore;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ScoreSystem.ResetHighScore();
        }
    }

    void UpdateHighScore()
    {
        _highScoreText.SetText($"High Score: { PlayerPrefs.GetInt("HighScore") } ");
    }
}