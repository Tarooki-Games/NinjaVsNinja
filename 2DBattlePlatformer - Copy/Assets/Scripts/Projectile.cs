using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    [SerializeField] float _launchForce = 4.0f;
    [SerializeField] float _bounceForce = 3.0f;
    [SerializeField] float _rotateSpeed = 1.0f;

    int _playerNumber = 0;
    public int PlayerNumber { get => _playerNumber; set => _playerNumber = value; }
    Player _player;
    public Player Player { get => _player; set => _player = value; }

    [SerializeField] int _bouncesRemaining = 5;
    [SerializeField] bool _isShuriken;
    [SerializeField] bool _canRotate = true;

    public bool IsShuriken => _isShuriken;

    public float Direction { get; set; }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rigidbody.velocity = new Vector2(_launchForce * Direction, _bounceForce / 2);
    }

    void Update()
    {
        if (_canRotate)
            transform.Rotate(0, 0, 360 * _rotateSpeed * Time.deltaTime * Direction);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float normalX = collision.GetContact(0).normal.x;
        
        ITakeDamage damageable = collision.collider.GetComponent<ITakeDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(_playerNumber, Player);
            Destroy(gameObject);
            return;
        }

        if (!_isShuriken)
        {
            if (normalX != 0)
            {
                Direction = normalX;
            }
            else
            {
                _bouncesRemaining--;
                if (_bouncesRemaining < 0)
                    Destroy(gameObject);
                else
                    _rigidbody.velocity = new Vector2(_launchForce * Direction, _bounceForce);
            }
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
            _canRotate = false;
            //Destroy(gameObject);
        }
    }
}
