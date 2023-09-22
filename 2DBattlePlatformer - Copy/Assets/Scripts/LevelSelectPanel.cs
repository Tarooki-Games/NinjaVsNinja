using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : MonoBehaviour
{
    void OnEnable()
    {
        PlayerPrefs.SetInt("CoinBattle_Unlocked", 1);
        //PlayerPrefs.SetInt("Level02_Unlocked", 1);
    }
    
    public void OpenPanel() => GetComponent<Animator>().SetBool("Open", true);
    public void ClosePanel() => GetComponent<Animator>().SetBool("Open", false);
}
