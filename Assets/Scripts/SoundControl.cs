using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundControl : MonoBehaviour {

    private AudioSource audioSource;
    private UnityEngine.Events.UnityAction action;
    private Button[] btns;

    void OnEnable () {
        // Find all the btns in the scene and add listeners to each
        btns = FindObjectsOfType<Button>();
        action = () => {HandleOnBtnClick();};

        foreach (Button btn in btns)
        {
            AddListener(btn);
        }
    }

    void OnDisable()
    {
        foreach (Button btn in btns)
        {
            RemoveListener(btn);
        }
    }
	
    private void AddListener(Button btn)
    {
        btn.onClick.AddListener(action);
    }

    private void RemoveListener(Button btn)
    {
        btn.onClick.RemoveListener(action);
    }

    private void HandleOnBtnClick()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip);
    }
}
