using System;
using System.Collections;
using UnityEngine;

public class FadingCloud : HittableFromBelow
{
    SpriteRenderer _spriteRenderer;
    Collider2D _collider2D;

    [SerializeField] float _resetDelay = 5f;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<Collider2D>();
    }

    protected override void Use( int playerNumber)
    {
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;

        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(_resetDelay);

        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
    }
}
