using UnityEngine;

public class UILockable : MonoBehaviour
{
    void OnEnable()
    {
        var startButton = GetComponent<UILevelButton>();
        string key = startButton.LevelName + "_Unlocked"; // "Level1_Unlocked"
        int unlocked = PlayerPrefs.GetInt(key, 0);
        
        // if (unlocked == 0)
        //     gameObject.SetActive(false);
    }

    [ContextMenu("Delete All PlayerPrefs")]
    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [ContextMenu("Lock Level (PlayerPrefs.DeleteKey")]
    void ClearLevelUnlocked()
    {
        var startButton = GetComponent<UILevelButton>();
        string key = startButton.LevelName + "_Unlocked";
        PlayerPrefs.DeleteKey(key);
    }

    //void OnValidate()
    //{
    //    if (!_newGameButton)
    //        GetComponentInChildren<TMP_Text>()?.SetText(LevelNumber().ToString());
    //}

    //int LevelNumber()
    //{
    //    if (_levelName == "Level01")
    //        return 1;
    //    else if (_levelName == "Level02")
    //        return 2;
    //    return 3;
    //}
}
