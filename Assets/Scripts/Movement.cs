using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float slowedSpeed = 0.5f;
    public float normalSpeed = 1f;
    public float maxLifeTime = 20;
    float currentSpeed;
    public float rotSpeed = 1.0f;
    private bool stageWon = false;
    private bool dead = false;
    public Color deathColor = new Color(255, 195, 0);
    public Color lifeColor = Color.white;
    public bool playerSlowed = false;
    Rigidbody2D rb;
    public Camera camera;
    Animator ani;
    private float timeSinceSpawn;
    private float drynessLevel;
    private SpriteRenderer sr;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Slow"))
        //{
        //    playerSlowed = true;
        //    var shake = camera.GetComponent<ScreenShake>();
        //    shake.shake = true;
        //    Debug.Log("player slowed");
        //}
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        //{
        //    Debug.Log("player dead");
        //    GameManager.instance.touchKillerThings();
        //}
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("WaterSource"))
        //{
        //    Debug.Log("player got some water, yum yum");
        //}
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = false;
            var shake = camera.GetComponent<ScreenShake>();
            shake.shake = false;
        }
        //if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //{
        //    Debug.Log("bounce of the wall");
        //}
        if (collision.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Debug.Log("player dead");
            GameManager.instance.touchKillerThings();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = true;
            var shake = camera.GetComponent<ScreenShake>();
            shake.shake = true;
            Debug.Log("player slowed");
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("WaterSource"))
        {
            sr.color = lifeColor;
            GameManager.instance.touchWater(collision.gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Deadly"))
        {
            Debug.Log("player dead");
            GameManager.instance.touchKillerThings();
        }
    }

    public void isDead()
    {
        dead = true;
        sr.color = deathColor;
    }

    public void gotWater()
    {
        this.stageWon = true;
        sr.color = Color.white;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = false;
            var shake = camera.GetComponent<ScreenShake>();
            shake.shake = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Slow"))
        {
            playerSlowed = false;
            var shake = camera.GetComponent<ScreenShake>();
            shake.shake = false;
        }
    }
}
