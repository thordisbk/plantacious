using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public float[] lifeTimePerWaterDrunk;

    public Slider waterSlider;
    private float sliderIncValue;
    //private bool raisingWaterSlider = false;
    //private float raisingWaterSliderValue = 0f;

    public Slider lifespanSlider;

    public int availableWaterSources = 4;
    private int waterDrunkCounter = 0;
    private float maxLifeTime = 5;
    private float currLifeTime = 0;

    public GameObject currPlayer;
    public GameObject curvedLineRendererPrefab;
    public GameObject plant;
    private Vector3[] spawnPoints;

    public GameObject playerPrefab;
    public GameObject currCurvedLineRenderer;

    private bool thingTouched = false;

    //public TextMeshProUGUI winText;
    public TextMeshProUGUI winText_0;
    public TextMeshProUGUI winText_1;
    public Image winOverlay;
    public Camera endCam;
    private Vector3 winScreenPos;
    public float winScreenPanTime = 5;
    public float fadeToBlackTime = 3;
    private bool gameWon = false;
    private bool gameWinScreenEnded = false;
    private float winTime;
    private Vector3 winPos;
    private Color lifeColor;
    private Color deathColor;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            print("destroy gamemanager");
            Destroy(gameObject);
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        initializeSettings();
        winScreenPos = new Vector3(0, 0, -35);
        plant = GameObject.Find("Plant");
    }

    void initializeSettings()
    {
        sliderIncValue = 1f / (float)availableWaterSources;
        waterSlider.value = 0f;

        //origPlayerPos = currPlayer.transform.position;
        lifeTimePerWaterDrunk = new float[availableWaterSources+1];
        lifeTimePerWaterDrunk[0] = 5;
        lifeTimePerWaterDrunk[1] = 10;
        lifeTimePerWaterDrunk[2] = 15;
        lifeTimePerWaterDrunk[3] = 25;
        lifeTimePerWaterDrunk[4] = 30;
        lifeTimePerWaterDrunk[5] = 35;
        lifeTimePerWaterDrunk[6] = 40;
        // TODO this needs to be changed if water sources are added

        maxLifeTime = lifeTimePerWaterDrunk[0];
        currPlayer.GetComponent<Movement>().maxLifeTime = maxLifeTime;
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().maxLifeTime = maxLifeTime;

        lifeColor = currPlayer.GetComponent<Movement>().lifeColor;
        deathColor = currPlayer.GetComponent<Movement>().deathColor;

    }

    void Update() {
        if (Input.GetKey("q") && Input.GetKey("w")) {
            WinGame();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkIfNoMoreTime();
        if (gameWon) updateWinCam();
        lifespanSlider.value = 1.0f - currPlayer.GetComponent<Movement>().getDrynessLevel();
    }

    private void checkIfNoMoreTime()
    {
        currLifeTime += Time.deltaTime;
        if (currLifeTime > maxLifeTime && !gameWon)
        {
            touchKillerThings();
        }
    }

    public void touchKillerThings() {
        Debug.Log("plant arm: kill");

        if (thingTouched) return;
        thingTouched = true;
        // stop the player
        currPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        currPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        currPlayer.GetComponent<Movement>().isDead();
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().isDead();

        // delay
        Invoke("stopOldGrowNewStem", 1f);
        // TODO keep the old, just change its color
    }
    
    public void touchWater(GameObject waterObj) {
        Debug.Log("plant arm: got water");

        if (thingTouched) return;
        thingTouched = true;

        // make water sound
        // TODO add sound

        // increase slider points
        // if (waterSlider.value + sliderIncValue > 1.0f) waterSlider.value = 1.0f; else 
        waterSlider.value = waterSlider.value + sliderIncValue;
        // TODO juice this slider

        // increase water drunk counter
        waterDrunkCounter++;

        currPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        currPlayer.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        // leave this player object here, spawn new player object at start
        // remove Movement component, remove first child
        // change old line renderer color
        currPlayer.GetComponent<Movement>().gotWater();
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().gotWater();
        // remove water collider
        Destroy(waterObj.GetComponent<CircleCollider2D>());
        Destroy(waterObj.GetComponent<BoxCollider2D>());

        if (waterDrunkCounter == availableWaterSources) {
            // TODO pan out or something
            Invoke("WinGame", 1f);
            return;
        }

        // delay
        Invoke("stopOldGrowNewStem", 1f);
    }

    private void stopOldGrowNewStem() {
        Vector3 spawnPoint = plant.transform.position;
        spawnPoint.y += 0.5f;

        Vector3[] poss = { spawnPoint,
                           spawnPoint,
                           spawnPoint,
                           spawnPoint,
                           spawnPoint,
                           spawnPoint};
        Vector3 [] rots = { new Vector3(0f, 0f, 0f), 
                            new Vector3(0f, 0f, -5f), 
                            new Vector3(0f, 0f, 14.96f), 
                            new Vector3(0f, 0f, 16.99f), 
                            new Vector3(0f, 0f, 36.45f), 
                            new Vector3(0f, 0f, 10.2f)};

        Vector3 theRot = new Vector3(0f, 0f, Random.Range(-15f, 15f));
        int i = Random.Range(0, 6);
        Vector3 newStemPos = poss[i];
        Quaternion newStemRot = Quaternion.Euler(theRot);
        Vector3 newStemPosLR = newStemPos;

        // player
        Destroy(currPlayer.GetComponent<Movement>());
        Destroy(currPlayer.GetComponent<Transform>().GetChild(0).gameObject);
        currPlayer = Instantiate(playerPrefab, newStemPos, newStemRot);
        // set new line renderer
        currCurvedLineRenderer = Instantiate(curvedLineRendererPrefab, newStemPosLR, Quaternion.identity);
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().plantGrowthGO = currPlayer.GetComponent<Transform>().GetChild(1).gameObject;
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().plantFaceGO = currPlayer.GetComponent<Transform>().GetChild(2).gameObject;
    
        thingTouched = false;

        maxLifeTime = lifeTimePerWaterDrunk[waterDrunkCounter];
        currPlayer.GetComponent<Movement>().maxLifeTime = maxLifeTime;
        currCurvedLineRenderer.GetComponent<CurvedLineRenderer>().maxLifeTime = maxLifeTime;

        currLifeTime = 0;
    }

    private void updateWinCam()
    {
        if (gameWinScreenEnded)
        {
            endCam.transform.position = winScreenPos;
            float t_ = (Time.fixedTime - winTime - winScreenPanTime) / fadeToBlackTime;
            winOverlay.color = Color.Lerp(new Color(winOverlay.color.r, winOverlay.color.g, winOverlay.color.b, 0.0f),
                                          new Color(winOverlay.color.r, winOverlay.color.g, winOverlay.color.b, 1.0f), t_);
            return;
        }
        float t = (Time.fixedTime - winTime) / winScreenPanTime;
        t = 1.0f - (1.0f - t) * (1.0f - t);
        endCam.transform.position = Vector3.Lerp(winPos, winScreenPos, t);
        //winText.color = Color.Lerp(new Color(winText.color.r, winText.color.g, winText.color.b, 0.0f),
        //                           new Color(winText.color.r, winText.color.g, winText.color.b, 1.0f), t);
        winText_0.color = Color.Lerp(new Color(winText_0.color.r, winText_0.color.g, winText_0.color.b, 0.0f),
                                     new Color(winText_0.color.r, winText_0.color.g, winText_0.color.b, 1.0f), t);
        winText_1.color = Color.Lerp(new Color(winText_1.color.r, winText_1.color.g, winText_1.color.b, 0.0f),
                                     new Color(winText_1.color.r, winText_1.color.g, winText_1.color.b, 1.0f), t);
        if (t > 0.95f) 
        { 
            gameWinScreenEnded = true;
            winScreenPos = endCam.transform.position;
        }
    }

    public void WinGame() {
        if (gameWon) return;
        gameWon = true;
        winTime = Time.fixedTime;
        winPos = Camera.main.transform.position;
        Destroy(Camera.main.gameObject);
        endCam.gameObject.SetActive(true);
        
        endCam.transform.position = winPos;
        //Camera.main.transform.SetParent(null);
        Invoke("changeScene", 10f);
    }

    void changeScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}


/*

new Vector3(-18.2f, 6.3f, 0f) 
new Vector3(0f, 0f, 0f)

new Vector3(-17.341f, 5.615f, 0f)
new Vector3(0f, 0f, 270f)

new Vector3(-17.794f, 6.335f, 0f)
new Vector3(0f, 0f, -14.96f)

new Vector3(-18.486f, 6.246f, 0f)
new Vector3(0f, 0f, 16.99f)

new Vector3(-18.749f, 5.993f, 0f)
new Vector3(0f, 0f, 36.45f)

new Vector3(-18.166f, 6.277f, 0f)
new Vector3(0f, 0f, 10.2f)
*/