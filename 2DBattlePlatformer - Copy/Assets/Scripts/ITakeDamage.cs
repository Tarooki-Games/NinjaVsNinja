using System;
using System.Collections;
using UnityEngine;

public interface ITakeDamage
{
    void TakeDamage(int playerFired, Player player);
}