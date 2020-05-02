using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// to use: in IEnumerator function, called with StartCoroutine
// call takeScreenshot.TakeScreenShot(500, 500);
// while (takeScreenshot.imageReady == false) {
//      yield return null;
// }
// use takeScreenshot.imageByteArray to upload or something


public class TakeScreenshot : MonoBehaviour
{
    public Camera cam;
    private bool takeScreenShotOnNextFrame = false;

    [HideInInspector] public byte[] imageByteArray;
    public bool imageReady = false;

    [HideInInspector] public bool saveScreenshot = false;

    // Start is called before the first frame update
    void Start()
    {
        //cam = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender() {
        if (takeScreenShotOnNextFrame) {
            takeScreenShotOnNextFrame = false;

            // take screenshot
            RenderTexture renderTexture = cam.targetTexture;
            // ARGB32 & RGBA32 (do not capture linerenderer), RGB24 (works but no alpha)
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            imageByteArray = renderResult.EncodeToPNG();

            // now save texture to png, jpg is also possible
            if (saveScreenshot) {
                string filename = "/screenshot" + System.DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".png";
                System.IO.File.WriteAllBytes(Application.dataPath + filename, imageByteArray);
                Debug.Log("saved screenshot.png");
                saveScreenshot = false;
            }
            
            // cleanup
            RenderTexture.ReleaseTemporary(renderTexture);
            cam.targetTexture = null;

            imageReady = true;
        }
    }

    public void TakeScreenShot(int width, int height, bool save=false) {
        int depthBuffer = 16;
        if (cam == null) print("cam is null");
        cam.targetTexture = RenderTexture.GetTemporary(width, height, depthBuffer);
        takeScreenShotOnNextFrame = true;
        if (save) saveScreenshot = true;
    }
}
