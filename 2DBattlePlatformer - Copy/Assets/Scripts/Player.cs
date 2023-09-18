using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, ITakeDamage
{
    Rigidbody2D _rigidBody2D;
    Collider2D _collider;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    AudioSource _audioSource;
    ProjectileLauncher _projLauncher;

    // VARIABLES
    [SerializeField] int _playerNumber = 1;

    [SerializeField] Transform _feet;
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;

    [Header("Movement")]
    [SerializeField] float _speed = 5f;
    [SerializeField] float _acceleration = 10f;
    [SerializeField] float _deceleration = 10f;
    [SerializeField] float _airAcceleration = 1f;
    [SerializeField] float _airDeceleration = 0.2f;
    [SerializeField] float _wallSlideSpeed = 1f;
    [SerializeField] float _slipFactor = 1f;
    [Header("Jump")]
    [SerializeField] int _maxJumps = 2;
    [SerializeField] float _jumpVelocity = 8f;
    [SerializeField] float _maxJumpDuration = 0.2f;
    [SerializeField] float _downPull = 0.2f;

    [Header("Controller Input")]
    float _horizontal;
    int _lastMoveDir = 1;
    bool _jump;

    Vector2 _startPosition;
    Vector2 _desiredVelocity;
    Vector2 _smoothedVelocity;
    bool _isGrounded;
    bool _isOnSlipperySurface;
    int _jumpsRemaining;
    float _fallTimer;
    float _jumpTimer;


    float _groundCheckRadius = 0.2f;
    string _jumpButton;
    string _horizontalAxis;

    // LayerMask Attributes
    int _propLayerMask;
    int _groundLayerMasks;
    int _iceGroundLayerMask;
    int _wallLayerMask;

    public int PlayerNumber => _playerNumber;
    public int LastMoveDir => _lastMoveDir;

    bool _canInput = true;
    bool _canMove = true;
    bool _canTakeDamage = true;
    
    bool _isAlive = true;

    [SerializeField] float _frequency = 10.0f;
    [SerializeField] float _timeStunned;
    [SerializeField] float _timeInvulnerable;

    float _stunTime;
    float _invulnerableTime;
    
    bool _isBattling;
    public bool IsBattling => _isBattling;
    
    int _heartCount = 3;
    public event Action<int, int> OnHeartLost;
    public event Action<int, int> OnHeartRecovered;
    
    // ANIMATOR
    static readonly int Walking = Animator.StringToHash("Walking");
    static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Slide = Animator.StringToHash("Slide");
    private static readonly int HorizontalMovement = Animator.StringToHash("HorizontalMovement");
    private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _projLauncher = GetComponent<ProjectileLauncher>();
        
        _coin = GameAssets.GetInstance()._coinFX;
        _slimyCoin = GameAssets.GetInstance()._slimyCoinFX;

        _stunTime = _timeStunned;
        _invulnerableTime = _timeInvulnerable;
    }

    void Start()
    {
        _startPosition = transform.position;
        _jumpsRemaining = _maxJumps;

        _jumpButton = $"P{_playerNumber}_Jump";
        _horizontalAxis = $"P{_playerNumber}_Horizontal";
        _propLayerMask = LayerMask.GetMask("Prop");
        _iceGroundLayerMask = LayerMask.GetMask("IceGround");
        _groundLayerMasks = LayerMask.GetMask("Ground", "IceGround", "Prop");
        _wallLayerMask = LayerMask.GetMask("Wall");

        _projLauncher.OnProjectileFired += Player_OnProjectileFired;
    }

    void Player_OnProjectileFired(object sender, System.EventArgs e)
    {
        _canInput = false;
        _canMove = false;
        _rigidBody2D.simulated = false;
        _collider.enabled = false;
        _animator.SetTrigger("Throwing");

        Vector2 startVelocity = _rigidBody2D.velocity;
        StartCoroutine(WaitForTime(0.5f, startVelocity));
    }

    IEnumerator WaitForTime(float timeToWait, Vector2 startVelocity)
    {
        yield return new WaitForSeconds(timeToWait);

        _rigidBody2D.velocity = Vector2.zero;
        _rigidBody2D.velocity = startVelocity;
        _canInput = true;
        _canMove = true;
        _rigidBody2D.simulated = true;
        _collider.enabled = true;
    }

    void Update()
    {
        //Debug.Log(BattleManager.GetInstance().IsBattling);
        if (BattleManager.GetInstance().IsBattling)
        {
            if (_isAlive)
            {
                UpdateSpriteDirection();

                // Ground Check
                UpdateIsGrounded();

                if (!_canMove)
                    return;

                // Get Player Input
                ReadHorizontaInput();

                if (_canInput)
                {
                    if (!_isOnSlipperySurface)
                        MoveHorizontal();
                    else
                        SlipHorizontal();
                }
                else
                {
                    _rigidBody2D.velocity = Vector2.zero;
                }

                UpdateAnimator();
                UpdateSpriteDirection();

                if (ShouldSlide())
                {
                    if (_jumpsRemaining < _maxJumps && _jumpsRemaining > 0)
                        _jumpsRemaining++;
                    else if (_jumpsRemaining <= 0)
                        _jumpsRemaining = 1;
                    ReadJumpInput();
                    if (ShouldJump())
                    {
                        //Debug.Log("ShouldJump = true");
                        WallJump();
                    }
                    else
                        WallSlide();

                    return;
                }

                ReadJumpInput();

                if (ShouldJump())
                    Jump();
                else if (ShouldContinueJump())
                    ContinueJump();

                UpdateAnimator();

                _jumpTimer += Time.deltaTime;

                if (_isGrounded && _fallTimer > 0.1f)
                {
                    _fallTimer = 0;
                    _jumpsRemaining = _maxJumps;
                }
                else
                {
                    var propCheck = Physics2D.OverlapCircle(_feet.position, 0.2f, _propLayerMask);
                    if (propCheck)
                    {
                        _jumpsRemaining = _maxJumps;
                        _fallTimer = 0;
                    }

                    _fallTimer += Time.deltaTime;
                    var downForce = _downPull * _fallTimer * _fallTimer;
                    if (downForce > 0.5f)
                        downForce = 0.5f;
                    _rigidBody2D.velocity =
                        new Vector2(_rigidBody2D.velocity.x, _rigidBody2D.velocity.y - downForce);
                }
            }
        }
    }


    void WallSlide()
    {
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, -_wallSlideSpeed);
    }

    bool ShouldSlide()
    {
        if (_isGrounded)
            return false;

        if (_rigidBody2D.velocity.y > 0)
            return false;

        if (_horizontal < 0)
        {
            var hit = Physics2D.OverlapCircle(_leftSensor.position, 0.1f, _wallLayerMask);
            if (hit != null)
                return true;
        }
        else if (_horizontal > 0)
        {
            var hit = Physics2D.OverlapCircle(_rightSensor.position, 0.1f, _wallLayerMask);
            if (hit != null)
                return true;
        }
        return false;
    }

    void WallJump()
    {
        // Reset velocity before adding jump force.
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        _rigidBody2D.velocity = new Vector2(-_horizontal * _jumpVelocity, _jumpVelocity * 1.5f);
    }

    void ReadJumpInput()
    {
        if (_canInput)
            _jump = Input.GetButtonDown(_jumpButton);
    }

    bool ShouldJump()
    {
        return _jump && _jumpsRemaining > 0;
    }

    void Jump()
    {
        // Reset velocity before adding jump force.
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0);
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _jumpVelocity);
        // _rigidBody2D.AddForce(Vector2.up * _jumpForce);
        _jumpsRemaining--;
        _fallTimer = 0;
        _jumpTimer = 0;

        if(_audioSource != null)
            _audioSource.Play();
    }

    bool ShouldContinueJump()
    {
        // Debug.Log(_jump && _jumpTimer <= _maxJumpDuration);
        return _jump && _jumpTimer <= _maxJumpDuration;
    }

    void ContinueJump()
    {
        _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _jumpVelocity);
        _fallTimer = 0;
    }

    void MoveHorizontal()
    {
        //####################//
        //  MOVEMENT HANDLER  //
        //####################//
        // Checks for either +1 or -1 using the absolute value.
        // Note: The absolute value of a number is always the same number without a negative sign.

        float smoothnessMultiplier = _horizontal == 0 ? _deceleration : _acceleration;

        if (!_isGrounded)
            smoothnessMultiplier = _horizontal == 0 ? _airDeceleration : _airAcceleration;

        var newHorizontal = Mathf.Lerp(_rigidBody2D.velocity.x, _horizontal * _speed, Time.deltaTime * smoothnessMultiplier);
        
        _rigidBody2D.velocity = new Vector2(newHorizontal, _rigidBody2D.velocity.y);

        //if (_horizontal > 0)
        //{
        //    _rigidBody2D.velocity = new Vector2(_speed, _rigidBody2D.velocity.y);
        //}
        //if (_horizontal < 0)
        //{
        //    _rigidBody2D.velocity = new Vector2(-_speed, _rigidBody2D.velocity.y);
        //}

        if (Input.GetAxis(_horizontalAxis) == 0 && !Physics2D.OverlapCircle(_feet.position, _groundCheckRadius, _iceGroundLayerMask))
        {
            _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);
        }
    }

    void SlipHorizontal()
    {
        //####################//
        //  MOVEMENT HANDLER  //
        //####################//
        // Checks for either +1 or -1 using the absolute value.
        // Note: The absolute value of a number is always the same number without a negative sign.
        _desiredVelocity = new Vector2(_horizontal * _speed, _rigidBody2D.velocity.y);

        _smoothedVelocity = Vector2.Lerp(_rigidBody2D.velocity, _desiredVelocity, Time.deltaTime / _slipFactor);
        _rigidBody2D.velocity = _smoothedVelocity;

        if (_rigidBody2D.velocity.x < 0.4f && _rigidBody2D.velocity.x > -0.4f && _horizontal == 0)
        {
            _rigidBody2D.velocity = Vector2.zero;
        }

        //if (Input.GetAxisRaw("Horizontal") == 0 && !Physics2D.OverlapCircle(_feet.position, _groundCheckRadius, LayerMask.GetMask("IceGround")))
        //{
        //    _rigidBody2D.velocity = new Vector2(0, _rigidBody2D.velocity.y);
        //}
    }

    void ReadHorizontaInput()
    {
        if (_canInput)
        {
            _horizontal = Input.GetAxis(_horizontalAxis); // Range: -1.0 to 1.0
            //Debug.Log($"_horizontal: {_horizontal}");
            if (_horizontal > 0)
                _lastMoveDir = 1;
            else if (_horizontal < 0)
                _lastMoveDir = -1;
        }
    }

    void UpdateAnimator()
    {
        bool walking = _horizontal >= 0.05 || _horizontal <= -0.05;
        _animator.SetBool(Walking, walking);
        _animator.SetBool(Jump1, ShouldContinueJump());
        _animator.SetBool(Slide, ShouldSlide());
        _animator.SetFloat(HorizontalMovement, _horizontal);
        // Debug.Log($"Jump: {ShouldContinueJump()}");
    }

    public void UpdateSpriteDirection()
    {
        if (_horizontal != 0)
        {
            // Accesses the flip (x) Attribute and sets it based on the right hand statement returning a bool
            _spriteRenderer.flipX = _lastMoveDir < 0;
        }
    }

    void UpdateIsGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(_feet.position, _groundCheckRadius, _groundLayerMasks);
        _isGrounded = (hit != null);
        _animator.SetBool(IsGrounded, _isGrounded);

        if (_isGrounded)
            _isOnSlipperySurface = hit.CompareTag("Slippery");
        else
            _isOnSlipperySurface = false;

        // _isOnSlipperySurface = hit?.CompareTag("Slippery") ?? false;
        // Debug.Log(_isGrounded);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_feet.position, _groundCheckRadius);
    }

    internal void ResetToStart()
    {
        _rigidBody2D.position = _startPosition;
        SceneManager.LoadScene("Menu");
    }


    internal void TeleportTo(Vector3 position)
    {
        _rigidBody2D.position = position;
        _rigidBody2D.velocity = Vector2.zero;
    }

    public void TakeDamageCheck()
    {
        if (_canTakeDamage)
        {
            //CombatScoreSystem.UpdatePoints(-1, PlayerNumber);
            _ = StartCoroutine(ReceiveDamage());
        }
    }

    IEnumerator ReceiveDamage()
    {
        _heartCount--;
        
        // Update UI - Heart Containers
        OnHeartLost?.Invoke(_playerNumber, _heartCount);
        
        if (_heartCount <= 0)
        {
            Die();
            Debug.Log("Die()");
            yield return new WaitForSeconds(5.0f);
            yield break;
        }

        Debug.Log("TakeDamage Called");
        _canTakeDamage = false;
        _animator.SetBool(Hurt, true);

        _canMove = false;
        _rigidBody2D.simulated = false;
        _collider.enabled = false;

        _audioSource.Play();
        // enabled = false; // this script.

        Debug.Log("StunTime Started");

        while (_stunTime > 0)
        {
            yield return null;
            _stunTime -= Time.deltaTime;
        }

        Debug.Log("StunTime Complete");

        _animator.SetBool(Hurt, false);

        _canMove = true;
        _rigidBody2D.velocity = Vector2.zero;
        _rigidBody2D.simulated = true;
        _collider.enabled = true;

        while (_invulnerableTime > 0)
        {
            yield return null;
            _invulnerableTime -= Time.deltaTime;

            // float waveFormula = Mathf.Cos(_frequency * Time.timeSinceLevelLoad + Mathf.PI + timeInvulnerable) + 1;

            //                        Cos(        x        *  frequency *    pi) * 0.5f + 0.5f
            float waveFormula = Mathf.Cos(_invulnerableTime * _frequency * Mathf.PI + Time.timeSinceLevelLoad) * 0.5f + 0.5f;
            //Debug.Log(waveFormula);

            waveFormula = Mathf.Clamp(waveFormula, 0.2f, 0.8f);
            //Debug.Log("Clamped: " + waveFormula);

            _spriteRenderer.color = new Color(1, 1, 1, waveFormula);
        }
        _spriteRenderer.color = Color.white;
        _stunTime = _timeStunned;
        _invulnerableTime = _timeInvulnerable;
        _canTakeDamage = true;
    }

    public void RecoverHeart()
    {
        if (_heartCount < 3)
        {
            _heartCount++;
            // Update UI - Heart Containers
            OnHeartRecovered?.Invoke(_playerNumber, _heartCount);
        }
    }

    [SerializeField] protected GameObject _coin;
    [SerializeField] protected GameObject _slimyCoin;

    public void TakeDamage(int playerFired, Player player)
    {
        TakeDamageCheck();
        
        Debug.Log("Projectile Hit Player");

        if (playerFired == 1)
        {
            _slimyCoin.GetComponent<SpriteRenderer>().color = Color.red;
            _coin.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (playerFired == 2)
        {
            _slimyCoin.GetComponent<SpriteRenderer>().color = Color.white;
            _coin.GetComponent<SpriteRenderer>().color = Color.white;
        }

        Debug.Log("   " + transform.position);
        if (playerFired == _playerNumber)
            return;
        
        _ = Instantiate(_coin, transform, false);
        
        Debug.Log($"P{playerFired} gained 1 Point");
        CombatScoreSystem.UpdatePoints(1, playerFired);
        CombatScoreSystem.UpdatePoints(-1, _playerNumber);

        //StartCoroutine(Die());
    }

    public event Action<int> OnPlayerDeath;
    
    void Die()
    {
        _canTakeDamage = false;
        _canInput = false;
        _animator.SetTrigger(Death);
        
        _isAlive = false;
        
        OnPlayerDeath?.Invoke(_playerNumber);
    }
}

