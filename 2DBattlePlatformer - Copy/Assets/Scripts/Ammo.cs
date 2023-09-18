using System.Collections;
using UnityEngine;

public class Ammo : PickUp
{
    [SerializeField] Projectile _projectilePrefab;
    [SerializeField] int _ammoCount = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;
        
        PickUpCollected();
        
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);

        player.GetComponent<ProjectileLauncher>().CollectAmmo(_projectilePrefab, _ammoCount);

        //OnAmmoCollected?.Invoke(player.PlayerNumber, _ammoCount);

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
            audioSource.Play();

        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}