using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Telemetry : MonoBehaviour
{
    // https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/viewform
    private const string GoogleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/";

    private const string GoogleFormBaseUrl_levelstart = "";
    private const string _gform_levelstart_timestamp = "";
    private const string _gform_levelstart_userid = "";
    private const string _gform_levelstart_levelnum = "";
    private const string _gform_levelstart_gameversion = "";
    private const string _gform_levelstart_ = "";

    private const string GoogleFormBaseUrl_watersource = "";
    private const string _gform_watersource_timestamp = "";
    private const string _gform_watersource_userid = "";
    private const string _gform_watersource_name = "";
    private const string _gform_watersource_timetoreach = "";
    private const string _gform_watersource_position = "";

    private const string GoogleFormBaseUrl_obstacleslow = "";
    private const string _gform_obstacleslow_timestamp = "";
    private const string _gform_obstacleslow_userid = "";
    private const string _gform_obstacleslow_name = "";
    private const string _gform_obstacleslow_timetoreach = "";

    private const string GoogleFormBaseUrl_obstacledeath = "";
    private const string _gform_obstacledeath_timestamp = "";
    private const string _gform_obstacledeath_userid = "";
    private const string _gform_obstacledeath_name = "";
    private const string _gform_obstacledeath_timetoreach = "";

    private const string GoogleFormBaseUrl_levelend = "";
    private const string _gform_levelend_timestamp = "";
    private const string _gform_levelend_userid = "";
    private const string _gform_levelend_timestampgamestarted = "";
    private const string _gform_levelend_numwatersourcesreached = "";
    private const string _gform_levelend_numobstacleslowtouched = "";
    private const string _gform_levelend_numobstacledeaths = "";
    private const string _gform_levelend_numtimeddeaths = "";
    private const string _gform_levelend_totalvines = "";
    private const string _gform_levelend_levelresult = "";
    private const string _gform_levelend_levelnum = "";
    private const string _gform_levelend_gameversion = "";

    private const string GoogleFormBaseUrl_image = "https://docs.google.com/forms/d/e/1FAIpQLSdyjvHloErswYvj-wZHXfWxBOi3sHlQZNMPgUwD0RqO9gDJQw/";
    private const string _gform_image_timestamp = "entry.2063940975";
    private const string _gform_image_image = "entry.936693460";

    private string USERID;

    public bool uploadFile = false;
    private bool fileUploadDone = false;

    public TakeScreenshot takeScreenshot;
    public bool saveScreenshot = true;

    private string timeFormatStr = "dd/MM/yyyy HH:mm:ss";


    void Start() {
        // TODO generate userid
        System.Guid myGUID = System.Guid.NewGuid();
        USERID = myGUID.ToString();
        Debug.Log("USERID: " + USERID);
    }

    void Update() {
        if (Input.GetKeyDown("space") && !fileUploadDone) {
            Debug.Log("Take screenshot");
            fileUploadDone = true;
            StartCoroutine(SubmitGoogleForm_UploadImage());
        }
    }

    public IEnumerator SubmitGoogleForm_watersource(string name, float timeToReach) {
        if (GoogleFormBaseUrl_watersource == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_watersource_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_watersource_userid, USERID);

            form.AddField(_gform_watersource_name, name);
            form.AddField(_gform_watersource_timetoreach, timeToReach.ToString());

            string urlGoogleFormResponse = GoogleFormBaseUrl_watersource + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log watersource: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_obstableslow(string name, float timeToReach) {
        if (GoogleFormBaseUrl_obstacleslow == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_obstacleslow_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_obstacleslow_userid, USERID);

            form.AddField(_gform_obstacleslow_name, name);
            form.AddField(_gform_obstacleslow_timetoreach, timeToReach.ToString());

            string urlGoogleFormResponse = GoogleFormBaseUrl_obstacleslow + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log obstacleslow: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_obstabledeath(string name, float timeToReach) {
        if (GoogleFormBaseUrl_obstacledeath == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_obstacledeath_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_obstacledeath_userid, USERID);

            form.AddField(_gform_obstacledeath_name, name);
            form.AddField(_gform_obstacledeath_timetoreach, timeToReach.ToString());

            string urlGoogleFormResponse = GoogleFormBaseUrl_obstacledeath + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log obstacledeath: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_levelstart(int lvlNum, string version) {
        if (GoogleFormBaseUrl_levelstart == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_levelstart_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_levelstart_userid, USERID);

            form.AddField(_gform_levelstart_levelnum, lvlNum.ToString());
            form.AddField(_gform_levelstart_gameversion, version);

            string urlGoogleFormResponse = GoogleFormBaseUrl_levelstart + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log levelstart: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_levelend() {
        if (GoogleFormBaseUrl_levelend == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_levelend_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_levelend_userid, USERID);

            //

            string urlGoogleFormResponse = GoogleFormBaseUrl_levelend + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log levelend: success");
            }
        }
    }

    // This will get our upload url and on the response we will start our coroutine to take the screenshot
    public IEnumerator SubmitGoogleForm_UploadImage() {
        if (GoogleFormBaseUrl_image == "") {
            yield return null;
        }
        else { 
            //print("Screen width: " + Screen.width + " and height: " + Screen.height);
            takeScreenshot.TakeScreenShot(Screen.width, Screen.height, saveScreenshot);

            // wait until the image is ready (so takeScreenshot.imageByteArray won't be empty)
            while (takeScreenshot.imageReady == false) {
                print("waiting for image...");
                yield return null;
            }
            Debug.Log("Image is ready: " + takeScreenshot.imageByteArray.Length + " bytes");
            string imageDataString = Convert.ToBase64String(takeScreenshot.imageByteArray);

            // create a Web Form, this will be the POST method's data
            var form = new WWWForm();
            form.AddField(_gform_image_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            //form.AddBinaryData("entry.936693460", takeScreenshot.imageByteArray, "screenshot.png", "image/png");  // or jpg
            form.AddField(_gform_image_image, imageDataString);

            string urlGoogleFormResponse = GoogleFormBaseUrl_image + "formResponse";
            // POST the screenshot to Google Forms
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("Upload image: success");
            }
        }
    }

    public void SaveScreenshot() {
        takeScreenshot.TakeScreenShot(Screen.width, Screen.height, true);
    }


    

}