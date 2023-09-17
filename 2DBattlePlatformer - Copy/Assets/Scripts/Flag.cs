using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Flag : MonoBehaviour
{
    [SerializeField] string _sceneName;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for Player
        var player = collision.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        // Trigger Animation if previous if statement is false aka player != null
        var animator = GetComponent<Animator>();
        animator.SetTrigger("Raise");

        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        PlayerPrefs.SetInt(_sceneName + "_Unlocked", 1);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(_sceneName);
    }
}
