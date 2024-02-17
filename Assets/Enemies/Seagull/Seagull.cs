using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements
using UnityEngine.Tilemaps;

public class Seagull : MonoBehaviour
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
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
         enemySr = gameObject.GetComponent<SpriteRenderer>();
        originalColor = enemySr.color;
    }


    private void Update()
    {
        currTime = Time.time;
        // Automatically trigger the dash if the cooldown is over
        if (canDash)
        {
            Dash();
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
