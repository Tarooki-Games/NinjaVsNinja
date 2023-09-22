using System;
using System.Collections;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] int _maxAmmoCount = 9;
    [SerializeField] int _currentAmmoCount = 0;
    [SerializeField] float _reloadTime = 1.0f;
    [SerializeField] float _muzzleDistance = 0.35f;
    [SerializeField] float _firingDelay = 0.2f;
    [SerializeField] ParticleSystem _warmPFXPrefab;
    [SerializeField] Transform _muzzle;

    Player _player;

    int _playerNumber;
    string _fireBtnName;
    float _nextShootTime;

    public event EventHandler OnProjectileFired;

    void Awake()
    {
        _player = GetComponent<Player>();
        _playerNumber = _player.PlayerNumber;
        _fireBtnName = $"P{_playerNumber}_Fire";
    }

    void Update()
    {
        if (BattleManager.GetInstance().IsBattling)
        {
            if (_playerNumber == 1 && Input.GetAxis(_fireBtnName) > 0)
            {
                if (HasAmmo && CanFire)
                    HandleFire();
            }
            else if (_playerNumber == 2 && Input.GetAxis(_fireBtnName) > 0)
            {
                if (HasAmmo && CanFire)
                    HandleFire();
            }
        }
    }

    bool CanFire => Time.time >= _nextShootTime;
    bool HasAmmo => _currentAmmoCount > 0;

    void HandleFire()
    {
        OnProjectileFired?.Invoke(this, EventArgs.Empty);
        StartCoroutine(Fire(_player.LastMoveDir));

        _nextShootTime = Time.time + _reloadTime;
    }

    
    public event Action<int, int> OnAmmoFired;
    IEnumerator Fire (float direction)
    {
        _currentAmmoCount--;
        OnAmmoFired?.Invoke(_playerNumber, _currentAmmoCount);
        
        // ParticleSystem warmUpPFX = Instantiate(_warmPFXPrefab, transform.position, Quaternion.identity);
        if (direction == -1)
            _muzzle.localPosition = new Vector3(-1.35f, 0, 0);
        else
            _muzzle.localPosition = new Vector3(1.35f, 0, 0);

        yield return new WaitForSeconds(_firingDelay);
        Projectile projectile = Instantiate(_projectilePrefab, new Vector3(transform.position.x + (_muzzleDistance * direction),
                                             transform.position.y, transform.position.z), Quaternion.identity);
        projectile.Direction = direction > 0 ? 1f : -1f;
        projectile.PlayerNumber = _playerNumber;
        projectile.Player = _player;
    }

    public event Action<int, bool, int> OnAmmoCollected;
    
    public void CollectAmmo(Projectile projectilePrefab, int ammoCount)
    {
        if (_projectilePrefab == projectilePrefab)
        {
            _currentAmmoCount += ammoCount;
            if (_currentAmmoCount > _maxAmmoCount)
                _currentAmmoCount = _maxAmmoCount;
        }
        else
        {
            if (projectilePrefab.IsShuriken)
                _maxAmmoCount = 9;
            else
                _maxAmmoCount = 18;
            
            _projectilePrefab = projectilePrefab;
            _currentAmmoCount = ammoCount;
        }
        
        OnAmmoCollected?.Invoke(_playerNumber, _projectilePrefab.IsShuriken, _currentAmmoCount);
    }
}
