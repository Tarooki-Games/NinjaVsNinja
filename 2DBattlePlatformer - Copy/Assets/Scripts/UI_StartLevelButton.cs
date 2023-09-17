using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_StartLevelButton : MonoBehaviour
{
    [SerializeField] bool _newGameButton;
    [SerializeField] string _levelName;

    public string LevelName => _levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(_levelName);
    }
}