using UnityEngine;
using System.Collections;

public class ChangeSceneColor : MonoBehaviour
{
    public Color color1 = Color.red;
    public Color color2 = Color.blue;
    public float duration = 3.0F;

    Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
    }

    public void ChangeColor()
    {
        float t = Mathf.PingPong(Time.time, duration) / duration;
        camera.backgroundColor = Color.Lerp(color1, color2, t);
    }
}
