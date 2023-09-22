using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UILevelButton : MonoBehaviour
{
    [SerializeField] bool mainButton;
    [SerializeField] string levelName;
    public string LevelName => levelName;
    
    [SerializeField] int levelValue;
    public int LevelValue => levelValue;

    bool _levelUnlocked;
    
    public event Action<int, int> OnNewLevelSelected;

    private void Awake()
    {
        if (mainButton)
            PlayerPrefs.SetString("SelectedLevel", levelName);
        else
        {
            //_button = GetComponent<Image>();
            _levelUnlocked = PlayerPrefs.GetInt(levelName + "_Unlocked") != 0;
            if (!_levelUnlocked)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("SelectedLevel"));
    }

    public void SetSelectedLevel()
    {
        if (PlayerPrefs.GetInt(levelName + "_Unlocked") == 1)
        {
            int currentlySelected = PlayerPrefs.GetInt("Selected");
            if (currentlySelected == levelValue)
                return;
            
            //_button.color = _selectedGreen;
            PlayerPrefs.SetString("SelectedLevel", levelName);
            PlayerPrefs.SetInt("Selected", levelValue);
            Debug.Log(PlayerPrefs.GetString("SelectedLevel"));
            if (PlayerPrefs.GetInt(levelName + "_Unlocked") == 1)
                OnNewLevelSelected?.Invoke(levelValue, currentlySelected);
        }
    }
}