using UnityEngine;
using System.Collections;

public static class TextureLoader{

    public static Texture2D MakeTexture(string texturePath)
    {
        Texture2D texture = Resources.Load(texturePath) as Texture2D;

        return texture;
    }

    public static Texture2D MakeTexture(int newImgWidth, int newImgHeight)
    {
        Texture2D texture = new Texture2D(newImgWidth, newImgWidth);

        return texture;
    }

}
