using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;

    public static GameAssets GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
    
    public Enemy[] _enemyPrefabs;
    public GameObject[] _pickUpPrefabs;
    
    public GameObject _coinFX;
    public GameObject _slimyCoinFX;

    public GameObject _pfAmmoEmpty;
    public GameObject _pfAmmoFireball;
    public GameObject _pfAmmoShuriken;
    
    public GameObject _pfHeartEmpty;
    public GameObject _pfHeartFull;
    
    public GameObject _pfDeadFace1;
    public GameObject _pfDeadFace2;

    public GameObject _pickUpSpawnFX;
    public GameObject _playerHitFX;
    public GameObject _fireballHitFX;
    public GameObject _shurikenHitFX;

    public GameObject _pfLockedLevel;
    public GameObject _pfUnlockedLevel;

}
