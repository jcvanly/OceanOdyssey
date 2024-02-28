using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SmallSquid : MonoBehaviour
{
    private Transform playerTransform;
    private float tentacleShootTimer;
    public float inkSpotShootInterval = 10f; // Interval for shooting ink spots
    private float inkSpotTimer;
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject inkSpotPrefab; // Reference to the Ink Spot prefab
    private bool isDead = false;
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    public float flashDuration = .2f;
    private SpriteRenderer enemySr;
    private Color damageColor = new Color(1f, 0f, 0f, 1f);
    private Color originalColor;
    private Color currColor;
    private float damageTime;
    private float timeLastFlash;
    private float currTime;
    private bool flashOnDamage = false;
    void Start()
    {
        inkSpotTimer = inkSpotShootInterval; // Initialize the ink spot timer
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemyDeath = GetComponent<EnemyDeath>();
        enemySr = gameObject.GetComponent<SpriteRenderer>();
        originalColor = enemySr.color;
    }
    void Update()
    {
        inkSpotTimer -= Time.deltaTime;
        currTime = Time.time;
        if (inkSpotTimer <= 0)
        {
            StartCoroutine(ShootInkSpot());
            inkSpotTimer = inkSpotShootInterval;
        }

        if(currTime >= (damageTime + flashDuration) && flashOnDamage == true)
        {
            resetColor();
        }

        else if (flashOnDamage == true)
        {
            currColor = enemySr.color;

            if(currColor == originalColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                enemySr.color = damageColor;
            }
            else if (currColor == damageColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                enemySr.color = originalColor;
            }
        }
    }


    IEnumerator ShootInkSpot()
    {
        Vector3 targetPosition = playerTransform.position; // Target position is current player position
        GameObject inkSpot = Instantiate(inkSpotPrefab, transform.position, Quaternion.identity); // Instantiate at Kraken's position

        // Assuming the ink spot moves directly towards the player's current position
        float speed = 5f; // Adjust speed as necessary
        while (inkSpot.transform.position != targetPosition)
        {
            if (inkSpot == null) yield break; // Exit if ink spot was destroyed for some reason
            inkSpot.transform.position = Vector3.MoveTowards(inkSpot.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        // Do not destroy the ink spot immediately after reaching the target. Wait for 5 seconds.
        yield return new WaitForSeconds(5f); // Wait for 5 seconds

        // Destroy the ink spot after 5 seconds without fading out
        if (inkSpot != null)
        {
            Destroy(inkSpot);
        }
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