using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ShareScreenshotAndroid : ShareScreenshotInterface
{
    IEnumerator ShareScreenshotInterface.ShareScreenshot(byte[] screenshot, bool isProcessing, string shareText, string gameLink, string subject, string imageName)
    {
        isProcessing = true;

        byte[] dataToSave = new byte[screenshot.Length];
        Array.Copy(screenshot, dataToSave, screenshot.Length);

        string destination = Path.Combine(Application.persistentDataPath, imageName + ".png");
        FileStream fs = new FileStream(destination, FileMode.OpenOrCreate);
        BinaryWriter ww = new BinaryWriter(fs);
        ww.Write(screenshot);
        ww.Close();
        fs.Close();

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + gameLink);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("setType", "image/png");
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        currentActivity.Call("startActivity", intentObject);

        isProcessing = false;

        yield return new WaitForSeconds(1);
    }
}
