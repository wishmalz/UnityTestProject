using UnityEngine;
using System.Collections;

public class LoadNewSceneOnClick : MonoBehaviour {

    public void LoadScene(int scene)
    {
        Application.LoadLevel(scene);
    }
}
