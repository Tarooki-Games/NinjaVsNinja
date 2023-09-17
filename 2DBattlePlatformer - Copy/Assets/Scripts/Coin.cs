using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static int CoinsCollected;

    [SerializeField] List<AudioClip> _clips;

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player == null)
            return;

        //gameObject.SetActive(false);

        CoinsCollected++;
        Debug.Log($"Total Coins Collected: {CoinsCollected}");

        ScoreSystem.Add(1);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        if (_clips.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, _clips.Count);
            AudioClip clip = _clips[randomIndex];
            GetComponent<AudioSource>().PlayOneShot(clip);
        }
        else
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
