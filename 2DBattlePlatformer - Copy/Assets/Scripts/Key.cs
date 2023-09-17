using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // [SerializeField] KeyLock _keyLock;
    [SerializeField] string _keyColour;

    AudioSource _audioSource;

    void Awake() => _audioSource = GetComponent<AudioSource>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        var keyLock = collision.GetComponent<KeyLock>();

        if (player == null && keyLock == null)
            return;

        if (player)
        {
            transform.SetParent(player.transform);
            transform.localPosition = Vector3.up;
            if (_audioSource != null)
                _audioSource.Play();
        }
        else if (keyLock)
        {
            if (_keyColour == keyLock.LockColour)
            {
                keyLock.Unlock();

                Destroy(gameObject);
            }
        }
    }
}
