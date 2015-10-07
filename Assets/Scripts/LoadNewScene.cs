using UnityEngine;
using System.Collections;

public class LoadNewScene : MonoBehaviour
{

    private AudioSource audioSource;
    private AudioClip clip;

    public void LoadScene(int scene)
    {
        StartCoroutine(playSoundThenLoad(scene));
    }

    IEnumerator playSoundThenLoad(int scene)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(audioSource.clip.length - (float)1.63);  // 1.63 length of silence in sound clip

        Application.LoadLevel(scene);
    }
}
