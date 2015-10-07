using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

public class TakeAndSendScreenshot : MonoBehaviour {

    private string shareText = "Example text!\n";
    private string gameLink = "Download the game on play store at " + "\nhttps://play.google.com/store/apps/details?id=com.TGC.guessthemovie&pcampaignid=GPC_shareGame";
    private string subject = "Rebus Guess The Movie Duck Type";
    private bool isProcessing = false;
    private ShareScreenshotInterface shareImg;

    public void SaveScreenshot(Camera screenshotCamera)
    {
        string saveToFileName = "Screenshots//Screenshot.png";
        Texture2D screenshot = TakeScreenshot(Screen.width, Screen.height, screenshotCamera);

        // choose object type based on platform
        if (Application.platform == RuntimePlatform.Android)
            shareImg = new ShareScreenshotAndroid();
        else
            shareImg = new ShareScreenshotAndroid();

        if (screenshot != null && saveToFileName != null)
        {
            byte[] bytes = null;
            
            if (saveToFileName.ToLower().EndsWith(".jpg"))
                bytes = screenshot.EncodeToJPG();
            else bytes = screenshot.EncodeToPNG();

            // share screenshot
            if (!isProcessing)
                StartCoroutine(shareImg.ShareScreenshot(bytes, isProcessing, shareText, gameLink, subject));
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
        Texture2D imgToAppend = TextureLoader.MakeTexture("Images/umbrella1");//Resources.Load("Images/umbrella1") as Texture2D;
        Texture2D textToAppend = TextureLoader.MakeTexture("Images/example_font_0"); //Resources.Load("Images/example_font_0") as Texture2D;
        Texture2D changedImg = null;
        int newImgHeight = 0;
        int newImgWidth = 0;

        newImgHeight = screenshot.height + imgToAppend.height;
        newImgWidth = screenshot.width;

        changedImg = new Texture2D(newImgWidth, newImgHeight);
        changedImg.SetPixels(0, changedImg.height - screenshot.height, screenshot.width, screenshot.height, screenshot.GetPixels());    // append sceenshot to new img
        changedImg.SetPixels(0, 0, imgToAppend.width, imgToAppend.height, imgToAppend.GetPixels());     // append custom img to new img
        changedImg.SetPixels(imgToAppend.width, 0, 14, 31, textToAppend.GetPixels(107, textToAppend.height-31, 14, 31));  // add a
        changedImg.SetPixels(imgToAppend.width + 14, 0, 14, 31, textToAppend.GetPixels(0, textToAppend.height - 31, 14, 31));   // add b
        changedImg.SetPixels(imgToAppend.width + 14 * 2 - 3, 0, 14, 31, textToAppend.GetPixels(30, textToAppend.height - 31, 14, 31));   // add 1

        return changedImg;
    }
}
