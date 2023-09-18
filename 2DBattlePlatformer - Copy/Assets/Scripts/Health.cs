using System.Collections;
using UnityEngine;

public class Health : PickUp
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(this + "OnTriggerEnter2D");
        
        var player = collision.GetComponent<Player>();
        if (player == null)
            return;

        Debug.Log(this + "OnTriggerEnter2D - after Player Check");
        
        PickUpCollected();

        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);

        player.RecoverHeart();

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