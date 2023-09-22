using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LevelSelectUIManager : MonoBehaviour
{
    [SerializeField] List<HoverEvents> levelBtnsHoverEventsList;
    [SerializeField] List<UILevelButton> levelSelectBtns;

    [SerializeField] Image levelSelectImage;
    [SerializeField] Sprite lockedLevelSprite;
    [SerializeField] Sprite unlockedLevelSprite;
    [SerializeField] TMP_Text selectedText;

    [SerializeField] int selected;
    Color32 _selectedGreen;

    private void Awake()
    {
        _selectedGreen = new Color32(180, 234, 0, 255);
        
        levelBtnsHoverEventsList = GetComponentsInChildren<HoverEvents>().ToList();
        for (int i = 0; i < levelBtnsHoverEventsList.Count; i++)
        {
            levelBtnsHoverEventsList[i].OnButtonHovered += LevelSelectUIManagerOnButtonHovered;
            levelBtnsHoverEventsList[i].OnButtonExited += LevelSelectUIManagerOnButtonExited;
        }

        levelSelectBtns = GetComponentsInChildren<UILevelButton>().ToList();
        for (int i = 0; i < levelSelectBtns.Count; i++)
        {
            levelSelectBtns[i].OnNewLevelSelected += LevelSelectUIManagerOnNewLevelSelected;
        }

        levelSelectImage = GetComponent<Image>();
        lockedLevelSprite = GameAssets.GetInstance()._pfLockedLevel.GetComponent<Image>().sprite;
        unlockedLevelSprite = GameAssets.GetInstance()._pfUnlockedLevel.GetComponent<Image>().sprite;
        selected = PlayerPrefs.GetInt("Selected");
    }

    void LevelSelectUIManagerOnNewLevelSelected(int newlySelected, int previouslySelected)
    {
        levelSelectBtns[newlySelected - 1].GetComponent<Image>().color = _selectedGreen;
        levelSelectBtns[previouslySelected - 1].GetComponent<Image>().color = Color.white;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
            PlayerPrefs.DeleteAll();
    }

    private void LevelSelectUIManagerOnButtonHovered(bool levelUnlocked, int level)
    {
        if (levelUnlocked)
        {
            levelSelectImage.sprite = unlockedLevelSprite;
            selectedText.SetText($"{level}");
        }
        else // isLocked
        {
            selectedText.SetText(" ");
            levelSelectImage.sprite = lockedLevelSprite;
        }
    }

    private void LevelSelectUIManagerOnButtonExited(bool levelUnlocked, int level)
    {
        levelSelectImage.sprite = unlockedLevelSprite;
        selectedText.SetText($"{PlayerPrefs.GetInt("Selected")}");
    }
}