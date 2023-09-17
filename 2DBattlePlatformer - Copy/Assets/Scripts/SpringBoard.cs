using UnityEngine;

public class SpringBoard : MonoBehaviour
{
    AudioSource _audioSource;
    SpriteRenderer _spriteRenderer;

    [SerializeField] Sprite _downSprite;
    [SerializeField] float _springVelocity = 10f;

    Sprite _upSprite;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _upSprite = _spriteRenderer.sprite;

        _audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            var rigidBody2D = player.GetComponent<Rigidbody2D>();
            if (rigidBody2D != null)
            {
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, _springVelocity);
                if (_audioSource != null)
                    _audioSource.Play();
                _spriteRenderer.sprite = _downSprite;
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            _spriteRenderer.sprite = _upSprite;
        }
    }
}
