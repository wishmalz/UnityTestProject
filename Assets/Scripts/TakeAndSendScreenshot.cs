using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using Soomla.Profile;
using Soomla;

public class TakeAndSendScreenshot : MonoBehaviour {

    private string shareText = "Ezample text!\n";
    private string gameLink = "Download the game on play store at " + "\nhttps://play.google.com/store/apps/details?id=com.TGC.guessthemovie&pcampaignid=GPC_shareGame";
    private string subject = "Rebus Guess The Movie Duck Type";
    private string imageName = "Screenshot"; // without the extension, for iinstance, MyPic 
    private bool isProcessing = false;

    public void SaveScreenshot(Camera screenshotCamera)
    {
        

        System.IO.Directory.CreateDirectory("Screenshots");
        string saveToFileName = "Screenshots//Screenshot" + ".png";//+ DateTime.Now.ToString(" yyyy-MM-dd-HH-mm-ss-fff") + ".png";
        Texture2D screenshot = TakeScreenshot(Screen.width, Screen.height, screenshotCamera);
        if (screenshot != null && saveToFileName != null)
        {
            byte[] bytes = null;
            if (Application.platform == RuntimePlatform.OSXPlayer ||
            Application.platform == RuntimePlatform.WindowsPlayer &&
            Application.platform != RuntimePlatform.LinuxPlayer
            || Application.isEditor)
            {
                if (saveToFileName.ToLower().EndsWith(".jpg"))
                    bytes = screenshot.EncodeToJPG();
                else bytes = screenshot.EncodeToPNG();
                FileStream fs = new FileStream(saveToFileName, FileMode.OpenOrCreate);
                BinaryWriter ww = new BinaryWriter(fs);
                ww.Write(bytes);
                ww.Close();
                fs.Close();
            }

            if (!isProcessing)
                StartCoroutine(ShareScreenshot(bytes));
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
        Texture2D textToAppend = Resources.Load("Images/example_font_0") as Texture2D;
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

    private IEnumerator ShareScreenshot(byte[] screenshot)
    {
        isProcessing = true;
        yield return new WaitForEndOfFrame();

        Texture2D screenTexture = new Texture2D(1080, 1080, TextureFormat.RGB24, true);
        screenTexture.Apply();

        byte[] dataToSave = screenshot;

        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        Debug.Log(destination);
        File.WriteAllBytes(destination, dataToSave);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + gameLink);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);

        }

        isProcessing = false;

    }
}
