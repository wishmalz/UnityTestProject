using UnityEngine;
using System.Collections;

interface ShareScreenshotInterface {
    IEnumerator ShareScreenshot(byte[] screenshot, bool isProcessing, string shareText, string gameLink, string subject, string imageName);
}
