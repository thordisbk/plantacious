﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Telemetry : MonoBehaviour
{
    private const string GoogleFormBaseUrl_levelstart = "https://docs.google.com/forms/d/e/1FAIpQLSfI2k2unmYYhXAvAkBElG1O6FsH7Z92_Vp9kbEJOHt9IDYRjA/";
    private const string _gform_levelstart_timestamp = "entry.240265877";
    private const string _gform_levelstart_userid = "entry.264234294";
    private const string _gform_levelstart_levelnum = "entry.1635703984";
    private const string _gform_levelstart_gameversion = "entry.1358507571";

    private const string GoogleFormBaseUrl_plantevent = "https://docs.google.com/forms/d/e/1FAIpQLScMgCv-AZyZNBQ2pMRDs3k-1ixAmkUZfG8FFiI6q7U8XQd-oQ/";
    private const string _gform_plantevent_timestamp = "entry.1002136411";
    private const string _gform_plantevent_userid = "entry.1071174944";
    private const string _gform_plantevent_type = "entry.1559315566";
    private const string _gform_plantevent_name = "entry.1340158559";
    private const string _gform_plantevent_currlifetime = "entry.2123958303";
    private const string _gform_plantevent_position = "entry.793292174";

    private const string GoogleFormBaseUrl_levelend = "https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/";
    private const string _gform_levelend_timestamp = "entry.1342095277";
    private const string _gform_levelend_userid = "entry.36639276";
    private const string _gform_levelend_gameversion = "entry.943134332";
    private const string _gform_levelend_timestampgamestarted = "entry.573473030";
    private const string _gform_levelend_levelnum = "entry.133326099";
    private const string _gform_levelend_levelresult = "entry.646337777";
    private const string _gform_levelend_totalvines = "entry.1142069857";
    private const string _gform_levelend_watersourcelist = "entry.1373471076";
    private const string _gform_levelend_numwatersourcesreached = "entry.508248308";
    private const string _gform_levelend_numobstacleslowtouched = "entry.1118109589";
    private const string _gform_levelend_numobstacledeaths = "entry.1478690419";
    private const string _gform_levelend_numtimeoutdeaths = "entry.809426293";

    private const string GoogleFormBaseUrl_image = "https://docs.google.com/forms/d/e/1FAIpQLSdyjvHloErswYvj-wZHXfWxBOi3sHlQZNMPgUwD0RqO9gDJQw/";
    private const string _gform_image_timestamp = "entry.2063940975";
    private const string _gform_image_userid = "entry.1279706829";
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

    public IEnumerator SubmitGoogleForm_plantevent(string type, string name, float currlifetime, Vector3 pos) {
        // type should be one of the following:
        // "watersource" "obstacledeath", "timeoutdeath", "obstacleslow"

        if (GoogleFormBaseUrl_plantevent == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_plantevent_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_plantevent_userid, USERID);

            form.AddField(_gform_plantevent_type, type);
            form.AddField(_gform_plantevent_name, name);
            form.AddField(_gform_plantevent_currlifetime, currlifetime.ToString());
            form.AddField(_gform_plantevent_position, pos.ToString());

            string urlGoogleFormResponse = GoogleFormBaseUrl_plantevent + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log plantevent: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_levelstart() {
        if (GoogleFormBaseUrl_levelstart == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_levelstart_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_levelstart_userid, USERID);

            form.AddField(_gform_levelstart_levelnum, GameManager.instance.levelNumber.ToString());
            form.AddField(_gform_levelstart_gameversion, GameManager.instance.gameVersion);

            string urlGoogleFormResponse = GoogleFormBaseUrl_levelstart + "formResponse";
            using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
            {
                yield return www.SendWebRequest();
                if (www.error != null) Debug.LogError(www.error);
                else Debug.Log("log levelstart: success");
            }
        }
    }

    public IEnumerator SubmitGoogleForm_levelend(string levelresult) {
        // levelresult can be:
        // "completed", "abandoned"

        if (GoogleFormBaseUrl_levelend == "") {
            yield return null;
        }
        else { 
            WWWForm form = new WWWForm();
            form.AddField(_gform_levelend_timestamp, System.DateTime.Now.ToString(timeFormatStr));
            form.AddField(_gform_levelend_userid, USERID);

            form.AddField(_gform_levelend_gameversion, GameManager.instance.gameVersion);
            form.AddField(_gform_levelend_timestampgamestarted, GameManager.instance.levelStartTime);
            form.AddField(_gform_levelend_levelnum, GameManager.instance.levelNumber.ToString());
            form.AddField(_gform_levelend_levelresult, levelresult);
            form.AddField(_gform_levelend_totalvines, GameManager.instance.vinesUsed.ToString());
            form.AddField(_gform_levelend_watersourcelist, String.Join(",", GameManager.instance.watersourcesReachedOrder.ToArray()));
            form.AddField(_gform_levelend_numwatersourcesreached, GameManager.instance.num_watersources.ToString());
            form.AddField(_gform_levelend_numobstacleslowtouched, GameManager.instance.num_obstacleslows.ToString());
            form.AddField(_gform_levelend_numobstacledeaths, GameManager.instance.num_obstacledeaths.ToString());
            form.AddField(_gform_levelend_numtimeoutdeaths, GameManager.instance.num_timeoutdeaths.ToString());

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
            form.AddField(_gform_image_userid, USERID);
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