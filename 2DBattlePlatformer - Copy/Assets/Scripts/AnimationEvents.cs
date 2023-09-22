using System;
using System.Collections;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public event Action OnFightCalledInAnim;
    public AudioSource mainMusic;

    private void Awake()
    {
        mainMusic = GetComponentInParent<AudioSource>();
    }

    public void FightCalledInAnim()
    {
        OnFightCalledInAnim?.Invoke();
    }

    public void PlayAnimSound(SoundManager.Sound sound)
    {
        GameObject soundGO = SoundManager.PlaySoundAndCleanUp(sound);
        _ = StartCoroutine(SoundCleanUp(soundGO, SoundManager.GetAudioClip(sound).length));
    }

    public IEnumerator SoundCleanUp(GameObject soundGO, float clipLength)
    {
        yield return new WaitForSeconds(clipLength + 0.5f);
        //Debug.Log(sound);
        Destroy(soundGO);
    }

    public void PlayAudioSource()
    {
        if (mainMusic == null)
            return;
        mainMusic.Play();
    }

    void DeactivateGameObject() => gameObject.SetActive(false);
}
