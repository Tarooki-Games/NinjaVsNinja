using TMPro;
using UnityEngine;

public class UIPlayerPrefsText : MonoBehaviour
{
    [SerializeField] string _key;
    int _value;
    TMP_Text _text;

    void OnEnable()
    {
        _text = GetComponent<TMP_Text>();
        _value = PlayerPrefs.GetInt(_key);
        _text.SetText(_value.ToString());
    }

    void Start()
    {
        ScoreSystem.OnHighScoreChanged += UpdateHighScore;
    }

    void UpdateHighScore()
    {
        _text.SetText($"High Score: { PlayerPrefs.GetInt("HighScore") } ");
    }
}
