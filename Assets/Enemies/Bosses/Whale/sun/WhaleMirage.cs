using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMirage : MonoBehaviour
{
    private bool shootDiagonal = true;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    private float shootTimer;
    public Sprite normalSprite; // Assign this in the Inspector
    public Sprite enragedSprite; // Assign this in the Inspector
    private bool isEnraged = false; // Tracks whether the mirage is enraged
    public WhaleBehavior mainWhale;


    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth; // Initialize current health
        GetComponent<SpriteRenderer>().sprite = normalSprite;
        
    }

    void Update()
    {
        if (mainWhale != null && mainWhale.isEnraged != isEnraged)
            {
                isEnraged = mainWhale.isEnraged;
                GetComponent<SpriteRenderer>().sprite = isEnraged ? enragedSprite : normalSprite;
            }
            
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }

        
    }

    void Shoot()
    {
        float speed = 4f;
        if (shootDiagonal)
        {
            // Shooting in all diagonal directions
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * speed;
        }
        else
        {
            // Shooting in cardinal directions
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1).normalized * speed;
        }

        // Toggle the firing pattern for the next call
        shootDiagonal = !shootDiagonal;
    }

    public void SetEnragedState(bool enraged)
    {
        isEnraged = enraged;
        GetComponent<SpriteRenderer>().sprite = isEnraged ? enragedSprite : normalSprite;
    }

}
