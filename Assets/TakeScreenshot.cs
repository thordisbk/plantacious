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
    private Camera camera;
    private bool takeScreenShotOnNextFrame = false;

    [HideInInspector] public byte[] imageByteArray;
    public bool imageReady = false;

    [HideInInspector] public bool saveScreenshot = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender() {
        if (takeScreenShotOnNextFrame) {
            takeScreenShotOnNextFrame = false;

            // take screenshot
            RenderTexture renderTexture = camera.targetTexture;
            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            imageByteArray = renderResult.EncodeToPNG();

            // now save texture to png, jpg is also possible
            if (saveScreenshot) {
                System.IO.File.WriteAllBytes(Application.dataPath + "/screenshot.png", imageByteArray);
                Debug.Log("saved screenshot.png");
                saveScreenshot = false;
            }
            
            // cleanup
            RenderTexture.ReleaseTemporary(renderTexture);
            camera.targetTexture = null;

            imageReady = true;
        }
    }

    public void TakeScreenShot(int width, int height, bool save=false) {
        int depthBuffer = 16;
        camera.targetTexture = RenderTexture.GetTemporary(width, height, depthBuffer);
        takeScreenShotOnNextFrame = true;
        if (save) saveScreenshot = true;
    }
}
