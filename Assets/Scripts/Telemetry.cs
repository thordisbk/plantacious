using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Telemetry : MonoBehaviour
{
    // https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/viewform
    private const string GoogleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfoys86Fr2kGrvP_2Y8Ny5SwGVRmrTvLw1CFapbbuM8-OZ16Q/";

    //private const string _gform_triggertimestamp = "entry.1115243052";
    //private const string _gform_triggeredbywhat = "entry.1306281063";
    private const string _gform_levelresult = "entry.646337777";

    public void RegisterCollision( string what )
    {
        StartCoroutine(SubmitGoogleForm(what) );
    }
    
    public IEnumerator SubmitGoogleForm(string data)
    {
        bool isNull = data is null;
        string jsonData = isNull ? "" : data;
        
        WWWForm form = new WWWForm();
        form.AddField(_gform_triggertimestamp, System.DateTime.Now.ToString());
        form.AddField(_gform_triggeredbywhat, data);

        string urlGoogleFormResponse = GoogleFormBaseUrl + "formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(urlGoogleFormResponse, form))
        {
            yield return www.SendWebRequest();
        }
    }
    
    
}