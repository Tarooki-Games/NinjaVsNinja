using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    protected override void Start()
    {
        base.Start();
        _spriteRenderer.flipX = Direction > 0;
    }

    protected override void TurnAround()
    {
        base.TurnAround();
        _spriteRenderer.flipX = Direction > 0;
    }
}
