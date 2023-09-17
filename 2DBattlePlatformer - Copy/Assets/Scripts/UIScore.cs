using TMPro;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    TMP_Text _scoreText;

    void Start()
    {
        _scoreText = GetComponent<TMP_Text>();
        ScoreSystem.OnScoreChanged += UpdateScore;
        UpdateScore(ScoreSystem.Score);
    }

    void OnDestroy()
    {
        ScoreSystem.OnScoreChanged -= UpdateScore;
    }

    void UpdateScore(int score)
    {
        _scoreText.SetText($"Score: { score } ");
    }
}
