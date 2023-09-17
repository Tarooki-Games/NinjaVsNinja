using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : MonoBehaviour
{
    void OnEnable()
    {
        PlayerPrefs.SetInt("Level01_Unlocked", 1);
    }

    public void OpenPanel() => GetComponent<Animator>().SetBool("Open", true);
    public void ClosePanel() => GetComponent<Animator>().SetBool("Open", false);
}
