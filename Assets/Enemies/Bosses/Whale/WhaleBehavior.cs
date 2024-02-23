using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WhaleBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int maxHealth = 100;
    public int currentHealth;
    public Transform playerTransform; // Make sure to assign the player's transform in the inspector
    public WhaleHealth healthBar;
    public Image fadePanel;
    public float fadeDuration = 2f;
    public GameObject victoryText;
    public GameObject nextIslandButton;
    public Image weatherIcon; // Reference to the UI element that will display the weather icons
    public Sprite sunIcon, rainIcon, snowIcon, windIcon; // References to the weather icon Sprites
    public Image weatherFilterPanel; // Add this line
    public GameObject rainParticleSystemPrefab;
    public GameObject snowParticleSystemPrefab;
    private GameObject currentWeatherEffect;
    private enum WeatherType { Rain, Sun, Wind, Snow }
    private WeatherType currentWeather = WeatherType.Rain; // Start with Rain
    public float chargeSpeed = 10f;
    public float attackDelay = 2f; // Time between attacks
    public PlayerHealth playerHealth;
    public Transform player; // Reference to the player's transform
    private Rigidbody2D rb;
    private float lastAttackTime = -1000; // Initialize to allow immediate attack
    private List<GameObject> projectilePool;
    public int poolSize = 32; // Adjust based on your needs
    private int currentProjectileIndex = 0;
    public GameObject whiteCirclePrefab; // Assign this in the Inspector
    private GameObject whiteCircleInstance;
    public GameObject lightningPrefab; // Assign this in the Inspector
    private bool _lightningCoroutineRunning = false;
    private float lastWindForceTime = 0f;
    private float windForceInterval = 2f; // Adjust the interval as needed
    private Vector2 windDirection = Vector2.zero;
    private bool isWindActive = false;
    public PhysicsMaterial2D icyMaterial;






    void Start()
{
    InitializeProjectilePool();
    HideVictoryScreen();
        currentHealth = maxHealth;
        currentWeather = WeatherType.Rain; // Ensure this is set before calling UpdateWeatherIcon
        UpdateWeatherIcon(); // Apply initial weather effect
        StartCoroutine(WeatherCycle());
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
}


    void Update()
    {
        if (currentWeather == WeatherType.Rain)
        {
            if (whiteCircleInstance == null)
            {
                // Instantiate the white circle above the player
                whiteCircleInstance = Instantiate(whiteCirclePrefab, player.position + Vector3.up * 2, Quaternion.identity);
            }
            else
            {
                // Move the white circle towards the player's position
                whiteCircleInstance.transform.position = Vector3.MoveTowards(whiteCircleInstance.transform.position, player.position, Time.deltaTime * 2f);
            }
        }
        else
        {
            // Optional: Destroy or deactivate the white circle when it's not raining
            if (whiteCircleInstance != null)
            {
                Destroy(whiteCircleInstance);
            }
        }

        if (currentWeather == WeatherType.Rain && whiteCircleInstance != null && !_lightningCoroutineRunning)
        {
            StartCoroutine(LightningStrikes());
        }

        else if ((currentWeather != WeatherType.Rain || whiteCircleInstance == null) && _lightningCoroutineRunning)
        {
            StopCoroutine(LightningStrikes());
            _lightningCoroutineRunning = false;
        }


        if (currentWeather == WeatherType.Sun && currentHealth > 0 && Time.time > lastAttackTime + attackDelay)
        {
            StartCoroutine(ShootFireBalls());
            lastAttackTime = Time.time;
            // Regain 10 HP but do not exceed maxHealth
            currentHealth = Mathf.Min(currentHealth + 10, maxHealth);
            healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar
        }


        if (currentWeather == WeatherType.Wind)
        {
            ApplyWindForceToPlayer();
        }


        if (currentWeather == WeatherType.Snow)
        {
            ApplyIcyConditions();
        }
        else
        {
            RemoveIcyConditions();
        }
    }

    IEnumerator WeatherCycle()
    {
        while (true)
        {
            // Change weather every 10 seconds
            yield return new WaitForSeconds(10f);
            ChangeWeather();
        }
    }

    void ChangeWeather()
    {
        // Cycle through the weather types
        Debug.Log("changing weather");
        currentWeather = (WeatherType)(((int)currentWeather + 1) % System.Enum.GetValues(typeof(WeatherType)).Length);
        UpdateWeatherIcon();
    }

void UpdateWeatherIcon()
{
    Color filterColor = Color.clear; // Default to clear

    // Destroy the current weather effect if it exists
    if (currentWeatherEffect != null)
    {
        Destroy(currentWeatherEffect);
    }

    Camera mainCamera = Camera.main;
    float topOfScreen = mainCamera.orthographicSize;
    Vector3 spawnPosition = mainCamera.transform.position 
                            + new Vector3(0, topOfScreen, 0);
    // Ensure the spawn position's z-coordinate is set appropriately to be visible
    spawnPosition.z = 0; // Adjust this value if necessary to match your 2D setup

    switch (currentWeather)
    {
        case WeatherType.Sun:
            weatherIcon.sprite = sunIcon;
            filterColor = new Color(1f, 0.64f, 0f, 0.3f);
            // No particle effect for sunny weather
            break;
        case WeatherType.Rain:
            weatherIcon.sprite = rainIcon;
            filterColor = new Color(0f, 0.5f, 1f, 0.3f);
            // Adjust spawn position to account for particle system size/shape if necessary
            currentWeatherEffect = Instantiate(rainParticleSystemPrefab, spawnPosition, Quaternion.identity);
            break;
        case WeatherType.Snow:
            weatherIcon.sprite = snowIcon;
            filterColor = new Color(1f, 1f, 1f, 0.3f);
            // Adjust spawn position to account for particle system size/shape if necessary
            currentWeatherEffect = Instantiate(snowParticleSystemPrefab, spawnPosition, Quaternion.identity);
            break;
        case WeatherType.Wind:
            weatherIcon.sprite = windIcon;
            filterColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            // Consider adding a wind effect or leave it as is for wind
            break;
    }

    weatherFilterPanel.color = filterColor; // Update the panel's color
}

 void InitializeProjectilePool()
    {
        projectilePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.SetActive(false); // Start with the projectile deactivated
            projectilePool.Add(proj);
        }
    }

    GameObject GetPooledProjectile()
    {
        GameObject proj = projectilePool[currentProjectileIndex];
        currentProjectileIndex = (currentProjectileIndex + 1) % poolSize;
        proj.SetActive(true); // Activate the projectile
        return proj;
    }

    IEnumerator LightningStrikes()
    {
        _lightningCoroutineRunning = true;
        while (whiteCircleInstance != null && currentWeather == WeatherType.Rain)
        {
            // Instantiate the lightning effect at the white circle's position
            Instantiate(lightningPrefab, whiteCircleInstance.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(2f); // Wait for 2 seconds before the next strike
        }
        _lightningCoroutineRunning = false;
    }
    IEnumerator ShootFireBalls()
    {
        float angleStep = 360f / 16;
        float angle = 0;

        for (int i = 0; i < 16; i++)
        {
            GameObject tmpObj = GetPooledProjectile();
            tmpObj.transform.position = transform.position; // Reset position to whale's position

            float projectileDirXposition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float projectileDirYposition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);
            Vector3 projectileVector = new Vector3(projectileDirXposition, projectileDirYposition, 0);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized * chargeSpeed;

            tmpObj.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            tmpObj.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(projectileMoveDirection.y, projectileMoveDirection.x) * Mathf.Rad2Deg);

            angle += angleStep;

            yield return new WaitForSeconds(0.05f); // Delay between each projectile spawn to spread out the performance impact
        }
    }

    void ApplyWindForceToPlayer()
    {
        if (!isWindActive || Time.time > lastWindForceTime + windForceInterval)
        {
            // Set a new random direction for the wind
            windDirection = new Vector2(Random.Range(-15f, 15f), Random.Range(-15f, 15f)); // Smaller values for a gradual push
            isWindActive = true;
            lastWindForceTime = Time.time;
        }
    }

    void FixedUpdate()
    {
        if (currentWeather == WeatherType.Wind && isWindActive)
        {
                    Debug.Log($"Applying wind force: {windDirection}");
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.AddForce(windDirection * 2f, ForceMode2D.Force); // Adjust force multiplier as needed
            }
        }
    }

    void ApplyIcyConditions()
    {

    Collider2D playerCollider = player.GetComponent<Collider2D>();
    if (playerCollider != null)
    {
        playerCollider.sharedMaterial = icyMaterial; // icyMaterial should have very low friction
    }

    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
    if (playerRb != null)
    {
        playerRb.drag = 0.01f; // Lower or remove drag to increase slipperiness
    }
    }

    void RemoveIcyConditions()
    {
        // Reset to default material or drag
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.sharedMaterial = null; // Assuming null resets to default
        }

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.drag = 0; // Reset drag
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
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // This method needs to be implemented in your HealthBar script

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {

        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }



        StartCoroutine(FadeToBlack());

    }

    public IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            Debug.Log(panelColor.a + elapsedTime);
            fadePanel.color = panelColor;
            yield return null;
        }

        ShowVictoryScreen();
        
        // Destroy the enemy object
        Destroy(gameObject);  
        
    }

    public void ShowVictoryScreen()
    {
        victoryText.SetActive(true);
        nextIslandButton.SetActive(true);
    }

    // Optionally, if you want to be able to hide them again
    public void HideVictoryScreen()
    {
        victoryText.SetActive(false);
        nextIslandButton.SetActive(false);
    }
}
