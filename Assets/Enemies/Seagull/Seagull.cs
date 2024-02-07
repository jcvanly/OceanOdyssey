using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.Tilemaps;

public class Seagull : MonoBehaviour
{
    public float dashSpeed = 7f;
    public float dashDuration = 1f;
    public float dashCooldown = 2f;

    private bool dashLeft = true;

    private bool canDash = true;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    //public Image healthBar; // Reference to the UI health bar
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    private float time;
    private float currTime;

    private float startDash;

    void Start()
    {
        time = Time.time;
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
    }

  //  void Update()
    //{

      //  if (canDash)
        //{
          //  Dash();
        //}

    //}

    void FixedUpdate()
    {
        if(canDash)
        {
            Dash();
        }
    }

    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10); // Assuming each hit decreases 10 health
            Destroy(collider.gameObject); // Destroy the projectile
        }

        if (collider.CompareTag("Obstacle"))
        {
            Debug.Log("seagull hit tile with c.");
            StopDash();
        }
    }

     private void Dash()
    {
        startDash = Time.time;
        canDash = false;

        Vector2 dashDirection;


        if(dashLeft == true)
        {
            dashDirection = new Vector2(1f, 0f);
            dashDirection.Normalize();
        }
        else
        {
            dashDirection = new Vector2(-1f, 0f);
            dashDirection.Normalize();
        }


        GetComponent<Rigidbody2D>().velocity = dashDirection * dashSpeed * Time.deltaTime;
    }

    private void StopDash()
    {
        if(dashLeft == true)
        {
            dashLeft = false;
        }
        else
        {
            dashLeft = true;
        }
        Debug.Log("Dash stop");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        canDash = true;
    }
    
    private void ResetDash()
    {
        canDash = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //healthBar.fillAmount = (float)currentHealth / maxHealth; // Update health bar

        if (currentHealth <= 0)
        {
            enemyDeath.Die();
            //Die();
        }
    }

    void Die()
    {
        // Add logic for enemy death, e.g., play animation, sound, etc.
        Destroy(gameObject); // Destroy the enemy object
    }
}
