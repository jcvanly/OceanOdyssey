using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferfish : MonoBehaviour
{
public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    private float shootTimer;
    private bool isDead = false;
    public float flashDuration = .2f;
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
        currTime = Time.time;
        shootTimer -= Time.deltaTime;
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
        Debug.Log("shoot");
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.down * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.left * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.right * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * 10f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * 10f;
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("entered 2D");
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            Debug.Log("taking 10 damage");
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
