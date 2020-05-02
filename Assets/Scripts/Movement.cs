using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float slowedSpeed = 0.5f;
    public float normalSpeed = 1f;
    public float maxLifeTime = 20f;
    private float currentSpeed;
    public float rotSpeed = 1.0f;
    private bool stageWon = false;
    private bool dead = false;
    public Color deathColor = new Color(255, 195, 0);
    public Color lifeColor = Color.white;
    public bool playerSlowed = false;
    
    private Rigidbody2D rb;
    public Camera cam;
    private Animator ani;
    private float timeSinceSpawn = 0f;
    private float drynessLevel;
    private SpriteRenderer sr;

    // set to true when on top of table, bed, etc to not collide 
    //  with moving things under the furniture (rats, roomba, etc)
    public bool onFurniture = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = normalSpeed;
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    public float getDrynessLevel()
    {
        return drynessLevel;
    }

    void ApplyDyingColor()
    {
        if (dead || stageWon) return;
        timeSinceSpawn += Time.deltaTime;
        drynessLevel = timeSinceSpawn / maxLifeTime;
        Color rootHeadColor = Color.Lerp(lifeColor, deathColor, drynessLevel);
        sr.color = rootHeadColor;
    }

    void Update()
    {
        if (dead || stageWon) return;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.forward, -rotSpeed * Time.deltaTime);
        }

        transform.GetChild(0).rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        if (dead || stageWon) return;
        ApplyDyingColor();
        if (playerSlowed)
        {
            ani.SetBool("isSlowed", playerSlowed);
            currentSpeed = slowedSpeed;
        }
        else
        {
            ani.SetBool("isSlowed", playerSlowed);
            currentSpeed = normalSpeed;
        }

        Vector2 velocity = transform.up * currentSpeed * Time.deltaTime;

        rb.velocity = velocity;

    }

    public void isDead()
    {
        dead = true;
        sr.color = deathColor;
        ani.SetBool("isSlowed", true);  // to set sad face
    }

    public void gotWater()
    {
        this.stageWon = true;
        sr.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = true;
            var shake = cam.GetComponent<ScreenShake>();
            shake.shake = true;
            Debug.Log("plant sprout slowed by: " + collision.gameObject.name);
            GameManager.instance.touchSlowThings(collision.gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("WaterSource"))
        {
            sr.color = lifeColor;
            GameManager.instance.touchWater(collision.gameObject);
        }

        if (!onFurniture && collision.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Debug.Log("plant sprout killed by: " + collision.gameObject.name);
            GameManager.instance.touchKillerThings(collision.gameObject);
        }
        else if (onFurniture && collision.gameObject.layer == LayerMask.NameToLayer("Deadly")) {
            Debug.Log("plant sprout on [object] collided with rat/roomba under [object]");
        }

        if (collision.gameObject.tag == "Climbable") {
            onFurniture = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Slow"))
        //{
        //    playerSlowed = true;
        //    var shake = cam.GetComponent<ScreenShake>();
        //    shake.shake = true;
        //    Debug.Log("player slowed");
        //}
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            Debug.Log("------- !! collided-slow");
            playerSlowed = false;
            var shake = cam.GetComponent<ScreenShake>();
            shake.shake = false;
        }
        if (!onFurniture && collision.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Debug.Log("plant sprout killed by: " + collision.gameObject.name);
            GameManager.instance.touchKillerThings(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = false;
            var shake = cam.GetComponent<ScreenShake>();
            shake.shake = false;
        }
        
        if (collision.gameObject.tag == "Climbable") {
            onFurniture = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = false;
            var shake = cam.GetComponent<ScreenShake>();
            shake.shake = false;
        }
    }
}
