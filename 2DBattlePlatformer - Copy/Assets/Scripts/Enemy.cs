using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage
{
    [SerializeField] Transform _leftSensor;
    [SerializeField] Transform _rightSensor;
    // [SerializeField] Transform _middleSensor;
    [SerializeField] Sprite _deadSprite;
    [SerializeField] private float _direction = -1;
    [SerializeField] bool _canFall = true;
    [SerializeField] float _jumpForce = 3.0f;
    [SerializeField] float _launchForce = 6.0f;
    [SerializeField] protected GameObject _coin;
    [SerializeField] protected GameObject _slimyCoin;
    [SerializeField] protected bool _isSlime = false;

    Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;
    protected Player _player;

    bool _isGrounded;

    public float Direction { get => _direction; set => _direction = value; }

    protected virtual void Start()
    {
        _coin = GameAssets.GetInstance()._coinFX;
        _slimyCoin = GameAssets.GetInstance()._slimyCoinFX;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        _rigidbody2D.velocity = new Vector2(Direction, _rigidbody2D.velocity.y);

        if (Direction < 0) // Going left
        {
            EnvironmentScan(_leftSensor);
        }
        else                // Going right
        {
            EnvironmentScan(_rightSensor);
        }

        if (transform.position.x <= -18 || transform.position.x >= +18)
            Destroy(gameObject);
    }

    protected virtual void EnvironmentScan(Transform sensor)
    {
        // Downward/Ground check
        Debug.DrawRay(sensor.position, Vector2.down * 0.2f, Color.blue);

        RaycastHit2D result = Physics2D.Raycast(sensor.position, Vector2.down, 0.54f);
        if (result.collider == null)
        {
            if (!_canFall)
                TurnAround();
            else if (_isGrounded)
            {
                RaycastHit2D groundCheck = Physics2D.Raycast(transform.position, Vector2.down, 0.54f);
                if (groundCheck.collider == null)
                {
                    _rigidbody2D.velocity = new Vector2(Direction, _jumpForce);
                    _isGrounded = false;
                    //Debug.Log(_isGrounded);
                }
            }
        }
        else
            _isGrounded = true;

        //Debug.Log(_isGrounded);

        Debug.DrawRay(sensor.position, new Vector2(Direction, 0) * 0.2f, Color.red);
        var forwardResult = Physics2D.Raycast(sensor.position, new Vector2(Direction, 0), 0.2f);
        if (forwardResult.collider != null && !forwardResult.collider.GetComponent<Player>() && forwardResult.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log(forwardResult.collider);
            TurnAround();
        }
    }

    protected virtual void TurnAround()
    {
        Direction *= -1;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        _player = collision.collider.GetComponent<Player>();
        if (_player == null)
            return;
        
        var contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        Debug.Log($"Normal = {normal}");

        if (normal.y <= -0.5)
        {
            TakeDamage(_player.PlayerNumber, _player);
        }
        else
            _player.TakeDamageCheck();
    }

    public virtual void TakeDamage(int playerFired, Player player)
    {
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

        //Debug.Log("   " + transform.position);
        if(!_isSlime)
            _ = Instantiate(_coin, transform, false);
        else
        {
            _ = Instantiate(_slimyCoin, transform, false);
        }
        CombatScoreSystem.UpdatePoints(1, playerFired);
        StartCoroutine(Die());
    }

    protected virtual IEnumerator Die()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _deadSprite;
        GetComponent<Animator>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<AudioSource>().Play();
        enabled = false; // this script.
        GetComponent<Rigidbody2D>().simulated = false;

        float alpha = 1;

        while (alpha > 0)
        {
            yield return null;
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(1, 1, 1, alpha);
        }
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
