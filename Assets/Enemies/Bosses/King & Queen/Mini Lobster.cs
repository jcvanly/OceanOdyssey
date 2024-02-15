using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.Tilemaps;

public class MiniLobster : MonoBehaviour
{
    public float dashSpeed = 5;
    public float dashDuration = 1f;
    public float dashCooldown = 3f;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector3 dashDirection;
    private bool canDash = true;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    //public Image healthBar; // Reference to the UI health bar
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    private float startDash;
    private bool isDead = false;
    

    void Start()
    {
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        // Automatically trigger the dash if the cooldown is over
        if (canDash)
        {
            Dash();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10);
        }
    }

    private void Dash()
    {
        canDash = false;

        dashDirection = player.transform.position - transform.position;
        rb.velocity = new Vector2(dashDirection.x,dashDirection.y).normalized * dashSpeed;
        
        dashDirection.Normalize();

        rb.velocity = dashDirection * dashSpeed;

        Invoke("StopDash", dashDuration);
        Invoke("ResetDash", dashCooldown);
    }

    private void StopDash()
    {
        Debug.Log("stopDash");
        rb.velocity = Vector2.zero;
    }

    private void ResetDash()
    {
        canDash = true;
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
        }
    }
}

