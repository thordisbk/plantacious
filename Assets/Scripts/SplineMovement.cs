using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurveType {
    Bezier,
    CatmullRom,
    Hermite,
    BSpline
}

public class SplineMovement : MonoBehaviour
{
    [Tooltip("Can use PointPrefab, place them in scene")]
    public List<Transform> points;
    public CurveType curveType = CurveType.CatmullRom;
    
    public float travelObjectSpeed = 1.0f;

    public bool closed = false;
    public bool moveTravelObject = false;
    public bool drawDv = false;
    public bool useNormalisedSpeed = true;
    public bool showPoints = true;

    private float travelObjectMarker = 0f; // position of travelObject in terms of t
    private float totalSplineLength = 0f;

    private CurveType lastChosenCurveType;
    private bool lastChosenClosed = false;
    private bool lastShowPoints;

    private List<Vector3> allPoints = new List<Vector3>();
    private List<Vector3> allGradients = new List<Vector3>();
    private List<float> pointsDistances = new List<float>();

    int numOfPoints = 0;

    public bool drawDebugLine = true;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastChosenCurveType = curveType;
        lastChosenClosed = closed;
        lastShowPoints = showPoints;
        foreach (Transform pt in points) {
            pt.hasChanged = false;
        }
        UpdatePoints(); 
    }

    // Update is called once per frame
    void Update()
    {
        bool changed = false;
        foreach (Transform pt in points) {
            if (pt.hasChanged && !changed) {
                UpdatePoints();
                pt.hasChanged = false;
                changed = true;
            }
        }
        if (curveType != lastChosenCurveType || closed != lastChosenClosed) {
            UpdatePoints();
        }
        if (showPoints && !points[0].GetComponent<SpriteRenderer>().enabled) {
            foreach (Transform p in points) {
                p.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else if (!showPoints && points[0].GetComponent<SpriteRenderer>().enabled) {
            foreach (Transform p in points) {
                p.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        lastChosenCurveType = curveType;
        lastChosenClosed = closed;
        lastShowPoints = showPoints;

        if (moveTravelObject) {
            travelObjectMarker += Time.deltaTime * travelObjectSpeed;
            float t = 0f;

            if (useNormalisedSpeed) { // normalise the speed of the object
                if (travelObjectMarker >= totalSplineLength) travelObjectMarker -= totalSplineLength;
                float offset = getNormalisedOffset(travelObjectMarker);
                t = offset;
            }
            else {
                if (travelObjectMarker >= (float) points.Count) travelObjectMarker = 0f;
                t = travelObjectMarker;
            }
            
            Vector3 point = getPointAtT(t);
            Vector3 gradient = getGradientAtT(t);
            transform.position = point;
            //rb.MovePosition(point);

            // for 3D
            Vector3 lookAtPoint = point - gradient;
            //transform.LookAt(lookAtPoint);

            // LookAt2D
            Vector2 targetPos = new Vector2(lookAtPoint.x - transform.position.x, lookAtPoint.y - transform.position.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg));
        }
    }

    void FixedUpdate() {
        if (!drawDebugLine) return;

        // so line will be drawn every fixed update and last until the next fixed update
        for (int i = 0; i < numOfPoints-1; i++) {    
            Debug.DrawLine(allPoints[i], allPoints[i+1], Color.white, 0.02f);

            if (drawDv && i % 15 == 0) {
                Vector3 point = allPoints[i];
                Vector3 gradient = allGradients[i];

                Vector3 newPoint = point + Vector3.Normalize(gradient);
                Debug.DrawLine(point, newPoint, Color.red, 0.02f);
            }

        }
    }

    void UpdatePoints() {

        allPoints.Clear();
        allGradients.Clear();
        pointsDistances.Clear();
        numOfPoints = 0;
        totalSplineLength = 0f;
        // Debug.Log("Update points");

        int endVal = points.Count - 3;

        float val = closed ? (float) points.Count : 1.0f;
        for (float t = 0f; t < val; t += 0.005f) {  // -3 because don't want to draw end points
            Vector3 p = getPointAtT(t);
            allPoints.Add(p);
            
            Vector3 dv_p = getGradientAtT(t);
            allGradients.Add(dv_p);

            numOfPoints++;
        }

        for (int i = 0; i < points.Count; i++) {
            float len = calcLength(i);
            totalSplineLength += len;
            pointsDistances.Add(len);
        }
        
        if (moveTravelObject) {
            // travelObjectMarker = 0f;
            transform.position = getPointAtT(travelObjectMarker);
        }
    }

    Vector3 getPointAtT(float t) {

        int i0 = 0, i1 = 0, i2 = 0, i3 = 0;  // indices

        if (closed) {
            i0 = ((int) t) % points.Count;

            int add = (int) t;
            if (curveType == CurveType.Bezier && add >= 1) {
                // i0, i1, i2, i3  // t = 0
                // i3, i0, i1, i2  // t = 1
                // i2, i3, i0, i1  // t = 2
                // i1, i2, i3, i0  // t = 3
                i0 = (3 * add) % points.Count;
            }

            i1 = (i0 + 1) % points.Count;
            i2 = (i1 + 1) % points.Count;
            i3 = (i2 + 1) % points.Count;
        }
        else {
            i0 = 0;
            i1 = 1;
            i2 = 2;
            i3 = 3;
        }
        // Debug.Log("i0 " + i0 + ", i1 " + i1 + ", i2 " + i2 + ", i3 " + i3);

        CurveSegment csX = new CurveSegment(curveType, points[i0].position.x, points[i1].position.x, points[i2].position.x, points[i3].position.x);
        CurveSegment csY = new CurveSegment(curveType, points[i0].position.y, points[i1].position.y, points[i2].position.y, points[i3].position.y);
        //CurveSegment csZ = new CurveSegment(curveType, points[i0].position.z, points[i1].position.z, points[i2].position.z, points[i3].position.z);

        float x = csX.Evaluate(t);
        float y = csY.Evaluate(t);
        //float z = csZ.Evaluate(t);
        //Vector3 p = new Vector3(x, y, z);
        Vector3 p = new Vector3(x, y, 0f);

        return p;
    }

    Vector3 getGradientAtT(float t) {

        int i0 = 0, i1 = 0, i2 = 0, i3 = 0;  // indices

        if (closed) {
            i0 = ((int) t) % points.Count;

            int add = (int) t;
            if (curveType == CurveType.Bezier && add >= 1) {
                i0 = (3 * add) % points.Count;
            }

            i1 = (i0 + 1) % points.Count;
            i2 = (i1 + 1) % points.Count;
            i3 = (i2 + 1) % points.Count;
            
        }
        else {
            i0 = 0;
            i1 = 1;
            i2 = 2;
            i3 = 3;
        }
        // Debug.Log("i0 " + i0 + ", i1 " + i1 + ", i2 " + i2 + ", i3 " + i3);

        CurveSegment csX = new CurveSegment(curveType, points[i0].position.x, points[i1].position.x, points[i2].position.x, points[i3].position.x);
        CurveSegment csY = new CurveSegment(curveType, points[i0].position.y, points[i1].position.y, points[i2].position.y, points[i3].position.y);
        CurveSegment csZ = new CurveSegment(curveType, points[i0].position.z, points[i1].position.z, points[i2].position.z, points[i3].position.z);

        float dv_x = csX.EvaluateDv(t);
        float dv_y = csY.EvaluateDv(t);
        float dv_z = csZ.EvaluateDv(t);
        Vector3 dv_p = new Vector3(dv_x, dv_y, dv_z);
        //Vector2 dv_p = new Vector2(dv_x, dv_y);
        return dv_p;
    }

    float calcLength(int i) {
        // returns the length between two points in points[]
        // i is the index of one of the points in points[]
        float len = 0f;
        //float steps = 0.005f;  // how many steps to iterate
        Vector3 oldpoint, newpoint;
        oldpoint = getPointAtT((float) i);  // get the first point

        // iterate along spline segment
        for (float t = 0f; t < 1f; t += 0.005f) {
            newpoint = getPointAtT((float) i + t);
            len += Vector3.Distance(oldpoint, newpoint);
            oldpoint = newpoint;
        }

        return len;
    }

    float getNormalisedOffset(float p) {
        // check which original point is the base
        int i;
        for (i = 0; p > pointsDistances[i]; i++) {
            p -= pointsDistances[i];
        }
        float frac = (float) i + (p / pointsDistances[i]);  // the offset
        return frac;
    }

}


