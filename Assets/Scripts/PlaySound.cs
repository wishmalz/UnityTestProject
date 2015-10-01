using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {
    private AudioSource audioSource;

    public void PlaySoundOnBtnClick(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.PlayOneShot(audioSource.clip);
    }
}
