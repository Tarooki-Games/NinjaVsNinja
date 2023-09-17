using System.Collections;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;

        float yVelocity = player.GetComponent<Rigidbody2D>().velocity.y;
        if (collision.contacts[0].normal.y > 0 && yVelocity >= 0)
            TakeHit();
    }

    void TakeHit()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        GetComponent<AudioSource>().Play();
    }
}
