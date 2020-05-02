using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScaleOverTime : MonoBehaviour
{
    float randomScale;
    
    void Awake()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }
    void Start()
    {
        randomScale = Random.Range(0.1f, 0.25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.localScale.x < randomScale) this.transform.localScale += new Vector3(Random.Range(0.00001f,0.0008f), 0, 0);
        if (this.transform.localScale.y < randomScale) this.transform.localScale += new Vector3(0, Random.Range(0.00003f, 0.0009f), 0); 
    }
}
