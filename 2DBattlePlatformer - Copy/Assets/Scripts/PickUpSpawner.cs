using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private Dictionary<int, Transform> _spawnPoints = new Dictionary<int, Transform>();
    [SerializeField] private Dictionary<int, bool> _hasPickUpAtPointDictionary = new Dictionary<int, bool>();

    [SerializeField] Timer _mainTimer;
    
    [SerializeField] protected GameObject[] _pickUpPrefabs;
    
    void Awake()
    {
        Transform[] spawnPoints = GetComponentsInChildren<Transform>();
        for (int i = 2; i < spawnPoints.Length; i++)
        {
            int key = i - 2;
            _spawnPoints.Add(key, spawnPoints[i]);
            _hasPickUpAtPointDictionary.Add(key, false);
        }

        // DEBUG CHECK for correct Transform Values stored at correct Key in Dictionary
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            Debug.Log($"Key: {i} - Value: {_spawnPoints[i]}");
            Debug.Log($"Key: {i} - Value: {_hasPickUpAtPointDictionary[i]}");
        }
        
        _mainTimer.OnIntervalAction += PickUpSpawnerOnInterval;
    }

    private void Start()
    {
        _pickUpPrefabs = GameAssets.GetInstance()._pickUpPrefabs;
    }

    void PickUpSpawnerOnInterval()
    {
        if (BattleManager.GetInstance().IsBattling)
        {
            if (ShouldSpawn())
            {
                // Do Spawn Stuff
                // Instantiate PFX

                // Instantiate Pick Up Object Prefab
                Spawn();
            }
        }
    }
    
    bool ShouldSpawn()
    {
        int pickUpsSpawned = 0;
        for (int i = 0; i < _hasPickUpAtPointDictionary.Count; i++)
        {
            if (_hasPickUpAtPointDictionary[i])
                pickUpsSpawned++;
        }

        Debug.Log($"pickUpsSpawned and in play: {pickUpsSpawned}");
        
        if (pickUpsSpawned < _hasPickUpAtPointDictionary.Count)
            return true;
        
        return false;
    }
    
    void Spawn()
    {
        int spawnPointKey = ChooseSpawnPoint();
        GameObject pickUpGO = ChoosePickUp();
        Transform spawnPoint = _spawnPoints[spawnPointKey];
        
        pickUpGO = Instantiate(pickUpGO, spawnPoint.position, transform.rotation);
        
        var pickUp = pickUpGO.GetComponent<PickUp>();
        pickUp._spawnPointKey = spawnPointKey;
        pickUp.OnPickUpCollected += PickUpSpawnerOnPickUpCollected;
    }

    void PickUpSpawnerOnPickUpCollected(int key)
    {
        Debug.Log($"PickUpSpawnerOnPickUpCollected() {key} - {false}");
        
        _hasPickUpAtPointDictionary[key] = false;
        
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            Debug.Log($"PickUpSpawnerOnPickUpCollected - Key: {i} - Value: {_hasPickUpAtPointDictionary[i]}");
        }
    }

    int ChooseSpawnPoint()
    {
        // 'UnityEngine.' can be removed if NOT also, 'using System;'.
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Count);
        
        //Check if point has PickUp at location
        while (_hasPickUpAtPointDictionary[randomIndex])
            randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Count);

        // spawnPoint is clear for PickUp to be spawned here, set to true.
        _hasPickUpAtPointDictionary[randomIndex] = true;
        
        //var spawnPoint = _spawnPoints[randomIndex];
        
        for (int i = 0; i < _hasPickUpAtPointDictionary.Count; i++)
        {
            Debug.Log($"Key: {i} - Value: {_hasPickUpAtPointDictionary[i]}");
        }
        
        return randomIndex;
    }

    GameObject ChoosePickUp()
    {
        int randomIndex = UnityEngine.Random.Range(0, _pickUpPrefabs.Length);
        var pickUp = _pickUpPrefabs[randomIndex];
        return pickUp;
    }
}
