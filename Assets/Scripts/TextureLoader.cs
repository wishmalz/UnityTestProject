﻿using UnityEngine;
using System.Collections;

public static class TextureLoader{

    public static Texture2D MakeTexture(string texturePath)
    {
        Texture2D texture = Resources.Load(texturePath) as Texture2D;

        return texture;
    }

}
