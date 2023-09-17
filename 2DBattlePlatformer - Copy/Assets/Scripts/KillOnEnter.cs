using System.Collections.Generic;
using UnityEngine;

public class KillOnEnter : MonoBehaviour
{
    //List<ParticleSystem.Particle> _particles;
    //ParticleSystem.ColliderData _colliderData;

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamageCheck();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamageCheck();
        }
    }
}
