using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UILevelButton _levelButton;
    bool _levelUnlocked;
    
    public event Action<bool, int> OnButtonHovered;
    public event Action<bool, int> OnButtonExited;

    private void Awake()
    {
        _levelButton = GetComponent<UILevelButton>();
        _levelUnlocked = PlayerPrefs.GetInt(_levelButton.LevelName + "_Unlocked") != 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnButtonHovered?.Invoke(_levelUnlocked, _levelButton.LevelValue);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnButtonExited?.Invoke(_levelUnlocked, _levelButton.LevelValue);
    }
}

