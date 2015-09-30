using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using Soomla.Profile;
using Soomla;

public class TakeAndSendScreenshot : MonoBehaviour {

    public void SaveScreenshot(Camera screenshotCamera)
    {
        System.IO.Directory.CreateDirectory("Screenshots");
        string saveToFileName = "Screenshots//Screenshot" + ".png";//+ DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss-fff") + ".png";
        Texture2D screenshot = TakeScreenshot(Screen.width, Screen.height, screenshotCamera);
        if (screenshot != null && saveToFileName != null)
        {
            if (Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WindowsPlayer &&
            Application.platform != RuntimePlatform.LinuxPlayer
            || Application.isEditor)
            {
                byte[] bytes;
                if (saveToFileName.ToLower().EndsWith(".jpg"))
                    bytes = screenshot.EncodeToJPG();
                else bytes = screenshot.EncodeToPNG();
                FileStream fs = new FileStream(saveToFileName, FileMode.OpenOrCreate);
                BinaryWriter ww = new BinaryWriter(fs);
                ww.Write(bytes);
                ww.Close();
                fs.Close();
            }
        }
    }

    private Texture2D TakeScreenshot(int width, int height, Camera screenshotCamera)
    {
        if (width <= 0 || height <= 0)
        {
            return null;
        }
        if (screenshotCamera == null) screenshotCamera = Camera.main;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
        RenderTexture renderTex = new RenderTexture(width, height, 24);
        screenshotCamera.targetTexture = renderTex;
        screenshotCamera.Render();
        RenderTexture.active = renderTex;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply(false);
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTex);
        screenshot = ChangeScreenshot(screenshot);
        return screenshot;
    }

    private Texture2D ChangeScreenshot(Texture2D screenshot)
    {
        Texture2D imgToAppend = Resources.Load("Images/umbrella1") as Texture2D;
        Texture2D textToAppend = Resources.Load("Images/example_font") as Texture2D;
        Texture2D changedImg = null;
        int newImgHeight = 0;
        int newImgWidth = 0;

        newImgHeight = screenshot.height + imgToAppend.height;
        newImgWidth = screenshot.width;

        changedImg = new Texture2D(newImgWidth, newImgHeight);
        changedImg.SetPixels(0, changedImg.height - screenshot.height, screenshot.width, screenshot.height, screenshot.GetPixels());    // append sceenshot to new img
        changedImg.SetPixels(0, 0, imgToAppend.width, imgToAppend.height, imgToAppend.GetPixels());     // append custom img to new img
        changedImg.SetPixels(imgToAppend.width, 0, 14, 31, textToAppend.GetPixels(99, textToAppend.height-31, 14, 31));  // add a
        changedImg.SetPixels(imgToAppend.width + 14, 0, 14, 31, textToAppend.GetPixels(0, textToAppend.height - 31, 14, 31));   // add b
        changedImg.SetPixels(imgToAppend.width + 13 * 2, 0, 14, 31, textToAppend.GetPixels(30, textToAppend.height - 31, 14, 31));   // add 1

        return changedImg;
    }
}
