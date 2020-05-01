using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Telemetry : MonoBehaviour
{
    // https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/viewform
    private const string GoogleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/";



    //private const string _gform_triggertimestamp = "entry.1115243052";
    //private const string _gform_triggeredbywhat = "entry.1306281063";
    private const string _gform_levelresult = "entry.646337777";
    private const string _gform_triggertimestamp = "";
    private const string _gform_image = "entry.688613739";

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
    private const string _gform_obstacleslow_position = "";

    private const string GoogleFormBaseUrl_obstacledeath = "";
    private const string _gform_obstacledeath_timestamp = "";
    private const string _gform_obstacledeath_userid = "";
    private const string _gform_obstacledeath_name = "";
    private const string _gform_obstacledeath_position = "";

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

    private string USERID;

    public bool uploadFile = false;
    private bool fileUploadDone = false;

    public TakeScreenshot takeScreenshot;


    void Start() {
        // TODO generate userid
    }

    void Update() {
        if (Input.GetKeyDown("space") && !fileUploadDone) {
            Debug.Log("Take screenshot");
            fileUploadDone = true;
            StartCoroutine(UploadImage());
        }
    }

    public void RegisterCollision( string what )
    {
        StartCoroutine(SubmitGoogleForm(what));
    }
    
    public IEnumerator SubmitGoogleForm(string data)
    {
        //bool isNull = data is null;
        //string jsonData = isNull ? "" : data;
        
        WWWForm form = new WWWForm();
        form.AddField(_gform_triggertimestamp, System.DateTime.Now.ToString());
        form.AddField(_gform_levelresult, data);

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
        }
    }

    public IEnumerator SubmitGoogleForm_watersource(string name, float timeToReach) {

        WWWForm form = new WWWForm();
        form.AddField(_gform_watersource_timestamp, System.DateTime.Now.ToString());
        form.AddField(_gform_watersource_name, name);
        form.AddField(_gform_watersource_timetoreach, timeToReach.ToString());
        form.AddField(_gform_watersource_userid, USERID);

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
        }
    }

    public IEnumerator SubmitGoogleForm_obstableslow(string name) {
        
        WWWForm form = new WWWForm();
        form.AddField(_gform_obstacleslow_timestamp, System.DateTime.Now.ToString());
        form.AddField(_gform_obstacleslow_name, name);
        form.AddField(_gform_obstacleslow_userid, USERID);

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
        }
    }

    public IEnumerator SubmitGoogleForm_obstabledeath(string name) {
        
        WWWForm form = new WWWForm();
        form.AddField(_gform_obstacledeath_timestamp, System.DateTime.Now.ToString());
        form.AddField(_gform_obstacledeath_name, name);
        form.AddField(_gform_obstacledeath_userid, USERID);

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
        }
    }


    //This will get our upload url and on the response we will start our coroutine to take the screenshot
    public void UploadScreenShot () {
        /*new GetUploadUrlRequest().Send((response) =>
        {
            //Start coroutine and pass in the upload url
            StartCoroutine(UploadAFile(response.Url));  
        });*/
        //StartCoroutine(UploadAFile("bleh"));  
    }

    //Our coroutine takes a screenshot of the game
    public IEnumerator UploadImage()
    {
        takeScreenshot.TakeScreenShot(500, 500, false);

        // wait until the image is ready (so takeScreenshot.imageByteArray won't be empty)
        while (takeScreenshot.imageReady == false) {
            yield return null;
        }
        Debug.Log("Image is ready: " + takeScreenshot.imageByteArray.Length + " bytes");
        string imageDataString = Convert.ToBase64String(takeScreenshot.imageByteArray);
        print(imageDataString);
        print(imageDataString.Length);

        // create a Web Form, this will be the POST method's data
        var form = new WWWForm();
        form.AddField("entry.2063940975", "hey hohoho");
        //form.AddBinaryData("entry.936693460", takeScreenshot.imageByteArray, "screenshot.png", "image/png");  // or jpg
        form.AddField("entry.936693460", imageDataString);
        Debug.Log("Upload image");

        string urlGoogleFormResponse = "https://docs.google.com/forms/d/e/1FAIpQLSdyjvHloErswYvj-wZHXfWxBOi3sHlQZNMPgUwD0RqO9gDJQw/" + "formResponse";
        // POST the screenshot to Google Forms
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
            if (www.error != null) Debug.Log(www.error);
            else Debug.Log("success ?");
        }
    }

    public void SaveScreenshot() {
        takeScreenshot.TakeScreenShot(500, 500, true);
    }


    

}