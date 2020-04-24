using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrap : MonoBehaviour
{
    public Sprite closedSprite;
    private Sprite openSprite;
    private bool closed = false;
    
    void Start()
    {
        openSprite = GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    { 
        
    }

    private void Close()
    {
        closed = true;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void Open()
    {
        closed = false;
        GetComponent<SpriteRenderer>().sprite = openSprite;
        gameObject.layer = LayerMask.NameToLayer("Deadly");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Close();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Close();
        
    }

}
