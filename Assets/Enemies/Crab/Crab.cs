using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.Tilemaps;

public class Crab : MonoBehaviour
{
    public GameObject projectilePrefab;
   // public Transform target;
    public float shootInterval = 2f;
    public int maxHealth = 70; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    private float shootTimer;
    private EnemyDeath enemyDeath;
    private bool isDead = false;
    public float flashDuration = .4f;
    private SpriteRenderer enemySr;
    private Color damageColor = new Color(1f, 0f, 0f, 1f);
    private Color originalColor;
    private float damageTime;
    private float timeLastFlash;
    private float currTime;
    private bool flashOnDamage = false;


    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        originalColor = enemySr.color;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        currTime = Time.time;
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }

        if(currTime >= (damageTime + flashDuration) && flashOnDamage == true)
        {
            resetColor();
        }
        else if (flashOnDamage == true)
        {
            if(enemySr.color == originalColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                enemySr.color = damageColor;
            }
            else if (enemySr.color == damageColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                enemySr.color = originalColor;
            }
        }
    }

    void Shoot()
    {
        float speed = 10f;
        // Shooting in all diagonal directions
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10); // Assuming each hit decreases 10 health
            Destroy(collider.gameObject); // Destroy the projectile
        }
    }
    

    public void TakeDamage(int damage)
    {
        if (isDead == false)
        {
            currentHealth -= damage;

 

            if (currentHealth <= 0)
            {
                enemyDeath.Die();
                isDead = true;
            }
            else
            {
                currTime = Time.time;
                flashOnDamage = true;
                damageTime = currTime;
                timeLastFlash = currTime;
                enemySr.color = damageColor;
            }
        }
    }

     public void resetColor()
    {
        enemySr.color = originalColor;
        flashOnDamage = false;
    }
}
