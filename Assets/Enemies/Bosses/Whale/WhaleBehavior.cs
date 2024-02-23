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
    private bool isCharging = false; // To prevent concurrent charges



    void Start()
{
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
        if (currentWeather == WeatherType.Rain && currentHealth > 0)
        {
            //StartCoroutine(RainPhaseAttack());
        }

        if (currentWeather == WeatherType.Sun && currentHealth > 0)
    {
        // Regain 10 HP but do not exceed maxHealth
        currentHealth = Mathf.Min(currentHealth + 10, maxHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar
        
        // Call your solar beam attack method here, ensure there's a cooldown
        if (!isCharging) // Assuming isCharging is used to prevent continuous attacks
        {
            StartCoroutine(SolarBeamAttack());
        }
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

IEnumerator SolarBeamAttack()
{
    Debug.Log("firing solar beam");
    isCharging = true;

    // Instantiate the solar beam projectile
    GameObject solarBeam = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

    // Calculate direction towards the player
    Vector2 direction = (playerTransform.position - transform.position).normalized;

    // Get the Rigidbody2D component of the solar beam and apply force
    Rigidbody2D rb = solarBeam.GetComponent<Rigidbody2D>();
    if (rb != null)
    {
        rb.AddForce(direction * chargeSpeed, ForceMode2D.Impulse);
    }

    // Wait for a delay before allowing another attack
    yield return new WaitForSeconds(attackDelay);

    isCharging = false;
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
        
        // Destroy all ink spots. Assuming you have a tag "InkSpot" for all ink spot objects
        foreach (GameObject inkSpot in GameObject.FindGameObjectsWithTag("InkSpot"))
        {
            Destroy(inkSpot);
        }

        foreach (GameObject tentacle in GameObject.FindGameObjectsWithTag("Tentacle"))
        {
            Destroy(tentacle);
        }

        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }


        GlobalEnemyManager.KrakenDefeated = true;

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
