using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnPoints;

    [SerializeField] Timer _mainTimer;
    
    [SerializeField] protected GameObject[] _pickUpPrefabs;
    
    void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().ToList();
        _spawnPoints.Remove(_spawnPoints[0]);
        _spawnPoints.Remove(_spawnPoints[0]);

        _mainTimer.OnIntervalAction += PickUpSpawnerOnInterval;
    }

    private void Start()
    {
        _pickUpPrefabs = GameAssets.GetInstance()._pickUpPrefabs;
    }

    void PickUpSpawnerOnInterval()
    {
        // Do Spawn Stuff
        // Instantiate PFX
        // Instantiate Pick Up Object Prefab
    }
    
    void Spawn()
    {
        Transform spawnPoint = ChooseSpawnPoint();

        GameObject pickUp = ChoosePickUp();
        Instantiate(pickUp, spawnPoint.position, transform.rotation);
    }
    
    Transform ChooseSpawnPoint()
    {
        // 'UnityEngine.' can be removed if NOT also, 'using System;'.
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Count);
        var spawnPoint = _spawnPoints[randomIndex];
        return spawnPoint;
    }

    GameObject ChoosePickUp()
    {
        int randomIndex = UnityEngine.Random.Range(0, _pickUpPrefabs.Length);
        var pickUp = _pickUpPrefabs[randomIndex];
        return pickUp;
    }
}
