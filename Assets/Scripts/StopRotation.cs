using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRotation : MonoBehaviour
{
    public bool stopRotation = true;
    private Quaternion rotation; 
    public bool restartTime = false;

    /*void Awake()
    {
        Vector3 parentRot = transform.parent.eulerAngles;
        Vector3 eulerRot = new Vector3(parentRot.x, parentRot.y, -1 * parentRot.z);
        rotation = Quaternion.Euler(eulerRot);
        transform.localEulerAngles = eulerRot;
        //transform.eulerAngles = 
        //Time.timeScale = 0f;

        Debug.Log("stem rot: " + parentRot + " cam rot: " + rotation.eulerAngles);
    }
    void LateUpdate()
    {
        if (restartTime) Time.timeScale = 1f;

        if (stopRotation) {
            transform.rotation = rotation;
        }
    }*/
}
