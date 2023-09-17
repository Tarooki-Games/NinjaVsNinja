using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float _bounceVelocity = 8f;

    AudioSource _audioSource;

    void Awake() => _audioSource = GetComponent<AudioSource>();

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            var rigidBody2D = player.GetComponent<Rigidbody2D>();
            if (rigidBody2D != null)
            {
                // Reset velocity before adding second jump force.
                // rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, _bounceVelocity);
                if (_audioSource != null)
                    _audioSource.Play();
            }
        }
    }
}
