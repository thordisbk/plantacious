using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class CurvedLineRenderer : MonoBehaviour
{
    //PUBLIC
    public GameObject plantGrowthGO;
    public GameObject plantFaceGO;
    public GameObject curvedLinePoint_prefab;
    public float pointInterval;
    public float lineSegmentSize = 0.15f;
    public float lineWidth = 0.1f;
    public float maxLifeTime = 20.0f;
    public Color deathColor = new Color(250,210,0);
    public Color lifeColor = Color.white;
    [Header("Gizmos")]
    public bool showGizmos = true;
    public float gizmoSize = 0.1f;
    public Color gizmoColor = new Color(1, 0, 0, 0.5f);
    //PRIVATE
    private List<GameObject> linePointList = new List<GameObject>();
    private GameObject[] linePointGOs = new GameObject[0];
    private Vector3[] linePositions = new Vector3[0];
    private Vector3[] linePositionsOld = new Vector3[0];
    private LeafGrower leafGrower;
    private LineRenderer line;
    private float timeSinceSpawn = 0;
    private bool stageWon = false;
    private bool dead = false;
    private float o3key = 0.6f;
    private float o7key = 0.9f;
    private bool o3goingUp = true;
    private bool o7goingUp = false;

    private float lifeTime = 0f;

    public void Start()
    {
        this.transform.position = plantGrowthGO.transform.position;
        leafGrower = GetComponent<LeafGrower>();
        AddNewPointToList(this.transform.position);

        lifeTime = 0f;
    }

    // Update is called once per frame
    void Update() {
        lifeTime += Time.deltaTime;
    }
    
    public void FixedUpdate()
    {
        GetPoints();
        SetPointsToLine();
        MaybeAddPoint();
        //if (lifeTime < GameManager.instance.waitBeforeMoving) return;
        ApplyColor();
    }

    void ApplyColor()
    {
        timeSinceSpawn += Time.deltaTime;
        float drynessLevel = timeSinceSpawn/maxLifeTime;
        float alpha = 1.0f;
        Color rootHeadColor = Color.Lerp(lifeColor,deathColor, drynessLevel);
        Color rootBodyColor = Color.Lerp(lifeColor, deathColor, drynessLevel);
        if (stageWon)
        {
            rootHeadColor = lifeColor;
            rootBodyColor = lifeColor;
        }

        if (dead)
        {
            rootHeadColor = deathColor;
            rootBodyColor = deathColor;
        }
        Gradient grad = new Gradient();
        grad.SetKeys(
            new GradientColorKey[] { new GradientColorKey(rootBodyColor, 0.0f), new GradientColorKey(rootHeadColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        line.colorGradient = grad;
    }
    void MaybeAddPoint()
    {
        int lastIndex = linePointList.Count-3;
        Vector3 lastPoint = linePointList[lastIndex].transform.position;
        Vector3 plantGrowthPoint = plantGrowthGO.transform.position;
        float distance = Vector3.Distance(lastPoint, plantGrowthPoint);
        if (distance > pointInterval)
        {
            Vector3 newPos = plantGrowthPoint;
            AddNewPointToList(newPos);
        }
    }
    void AddNewPointToList(Vector3 position)
    {
        GameObject cvp = Instantiate(curvedLinePoint_prefab);
        cvp.transform.position = position;
        cvp.transform.SetParent(this.transform);
        linePointList.Remove(plantGrowthGO);
        linePointList.Remove(plantFaceGO);
        linePointList.Add(cvp);
        linePointList.Add(plantGrowthGO);
        linePointList.Add(plantFaceGO);

        Vector3 directionToHead = plantFaceGO.transform.position - position;
        leafGrower.growLeaves(position, directionToHead);
    }

    void GetPoints()
    {
        //add positions to array
        linePointGOs = linePointList.ToArray();

        linePositions = new Vector3[linePointGOs.Length];
        for (int i = 0; i < linePointGOs.Length; i++)
        {
            linePositions[i] = linePointGOs[i].transform.position;
        }
    }

    void SetPointsToLine()
    {
        //create old positions if they dont match
        if (linePositionsOld.Length != linePositions.Length)
        {
            linePositionsOld = new Vector3[linePositions.Length];
        }

        //check if line points have moved
        bool moved = false;
        for (int i = 0; i < linePositions.Length; i++)
        {
            //compare
            if (linePositions[i] != linePositionsOld[i])
            {
                moved = true;
            }
        }

        //update if moved
        if (moved == true)
        {
            line = this.GetComponent<LineRenderer>();
            //get smoothed values
            Vector3[] smoothedPoints = LineSmoother.SmoothLine(linePositions, lineSegmentSize);

            //set line settings
            line.positionCount = smoothedPoints.Length;
            line.SetPositions(smoothedPoints);
            AnimationCurve curve = getAnimationCurve();
            line.widthCurve = curve;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
        }
    }

    AnimationCurve getAnimationCurve()
    {
        newKeyValue();
        AnimationCurve animationCurve = new AnimationCurve();
        animationCurve.AddKey(0, 0.5f);
        animationCurve.AddKey(0.3f, o3key);
        animationCurve.AddKey(0.6f, o7key);
        animationCurve.AddKey(1, 1);

        return animationCurve;
    }

    void newKeyValue()
    {
        if (o3key <= 0.6f) o3goingUp = true;
        if (o3key >= 0.9f) o3goingUp = false;

        if (o7key <= 0.5f) o7goingUp = true;
        if (o7key >= 0.9f) o7goingUp = false;

        if (o3goingUp) o3key += 0.01f;
        if (!o3goingUp) o3key -= 0.01f;
        if (o7goingUp) o7key += 0.015f;
        if (!o7goingUp) o7key -= 0.015f;
    }

    public Vector3 getLatestGrowPoint()
    {
        return linePointList[linePositions.Length-2].transform.position;
    }

    public void gotWater()
    {
        this.stageWon = true;
        ApplyColor();
    }

    public void isDead()
    {
        this.dead = true;
        ApplyColor();
    }
}
