using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float _nextSpawnTime;
    [SerializeField] protected float _delay = 3f;
    // [SerializeField] GameObject _prefab;
    [SerializeField] protected Transform[] _spawnPoints;
    [SerializeField] protected Enemy[] _enemyPrefabs;
    [SerializeField] protected Animator[] _doorAnimators;
    [SerializeField] protected SpriteRenderer[] _doors;

    [SerializeField] protected List<Enemy> _enemies;
    
    Sprite _startSprite;

    // It worked without the virtual/override void Start aka the private void Start(), but it gave a weird suggestion
    // to pull the Start() up into the base Spawner script. When all private void Start() in the inheriting spawners, so I went with this.
    void Start()
    {
        _startSprite = _doors[0].sprite;
        // _delay = 3f;
        // _prefab = GameAssets.GetInstance().pfGoldRobot;
        _enemyPrefabs = GameAssets.GetInstance()._enemyPrefabs;
        _doorAnimators = GetComponentsInChildren<Animator>();

        BattleManager.GetInstance().OnWinConditionMet += EnemySpawnerOnWinConditionMet;

        //for (int i = 0; i < _doorAnimators.Length; i++)
        //Debug.Log($"Name: {_doorAnimators[i]}   Index: {i} ");

        // Fills the array with all of the children components. The LAMBDA expression expressly ensures that only children are 
        // included and NOT the parent. The alternative is to just not use _spawnPoints[0] and set range from 1, see line 34, 
        // which seems more efficient in this case, see line 20, but I wanted to use a Lambda function. also, more expandable
        // because I or anyone else, wouldn't have to remember to account for the parent being in the array.
        // _spawnPoints = System.Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform);
        // _spawnPoints = GetComponentsInChildren<Transform>();        
    }

    void EnemySpawnerOnWinConditionMet(int winner, int winCondition)
    {
        for(int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].OnDestroy -= EnemySpawnerOnDestroy;
            Destroy(_enemies[i].gameObject);
        }
        _enemies.Clear();
    }

    void Update()
    {
        if (BattleManager.GetInstance().IsBattling)
        {
            if (ShouldSpawn())
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        _nextSpawnTime = Time.time + _delay;
        Transform spawnPoint = ChooseSpawnPoint();

        StartCoroutine(AnimateDoorAndSpawnEnemy(spawnPoint));
    }

    IEnumerator AnimateDoorAndSpawnEnemy(Transform spawnPoint)
    {
        if (spawnPoint == _spawnPoints[0])
        {
            _doorAnimators[0].SetTrigger("Warning");
            yield return new WaitForSeconds(1.29f);
            _doorAnimators[0].SetBool("OpenDoor", true);
            yield return new WaitForSeconds(0.45f);
            Enemy enemy = Instantiate(ChooseEnemy(), spawnPoint.position, transform.rotation);
            enemy.Direction = -1;
            enemy.OnDestroy += EnemySpawnerOnDestroy;
            _enemies.Add(enemy);
            yield return new WaitForSeconds(0.6f);
            _doorAnimators[0].SetBool("OpenDoor", false);
            _doors[0].sprite = _startSprite;
        }
        else
        {
            _doorAnimators[1].SetTrigger("Warning");
            yield return new WaitForSeconds(1.29f);
            _doorAnimators[1].SetBool("OpenDoor", true);
            yield return new WaitForSeconds(0.45f);
            Enemy enemy = Instantiate(ChooseEnemy(), spawnPoint.position, transform.rotation);
            enemy.Direction = 1;
            enemy.OnDestroy += EnemySpawnerOnDestroy;
            _enemies.Add(enemy);
            yield return new WaitForSeconds(0.6f);
            _doorAnimators[1].SetBool("OpenDoor", false);
            _doors[1].sprite = _startSprite;
        }
    }

    private void EnemySpawnerOnDestroy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        enemy.OnDestroy -= EnemySpawnerOnDestroy;
        Destroy(enemy.gameObject);
    }

    bool ShouldSpawn()
    {
        return Time.time >= _nextSpawnTime;
    }

    Transform ChooseSpawnPoint()
    {
        // 'UnityEngine.' can be removed if NOT also, 'using System;'.
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
        var spawnPoint = _spawnPoints[randomIndex];
        return spawnPoint;
    }

    Enemy ChooseEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemyPrefabs.Length);
        var enemy = _enemyPrefabs[randomIndex];
        return enemy;
    }

    private void OnDisable()
    {
        BattleManager.GetInstance().OnWinConditionMet -= EnemySpawnerOnWinConditionMet;
    }
}
