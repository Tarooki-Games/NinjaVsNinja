using System;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int _spawnPointKey;

    public event Action<int> OnPickUpCollected;
    
    protected void PickUpCollected()
    {
        //_collected = true;
        OnPickUpCollected?.Invoke(_spawnPointKey);
    }
}