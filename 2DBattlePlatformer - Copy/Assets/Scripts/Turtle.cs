using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    protected override void Start()
    {
        base.Start();
        _spriteRenderer.flipX = Direction < 0;
    }

    protected override void TurnAround()
    {
        base.TurnAround();
        _spriteRenderer.flipX = Direction < 0;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<Player>();
        if (player == null)
            return;
        var contact = collision.contacts[0];
        Vector2 normal = contact.normal;

        //Debug.Log($"Normal = {normal}");

        if (normal.y <= 0.5)
        {
            player.TakeDamageCheck();
            TurnAround();
        }
        else
        {
            TakeDamage(player.PlayerNumber, player);
        }
    }
}
