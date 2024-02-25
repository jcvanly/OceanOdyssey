using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WhaleBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public int maxFireballs = 48; 
    public int maxHealth = 100;
    public int currentHealth;
    public Transform playerTransform; 
    public WhaleHealth healthBar;
    public Image fadePanel;
    public float fadeDuration = 2f;
    public GameObject victoryText;
    public GameObject nextIslandButton;
    public Image weatherIcon; // Reference to the UI element that will display the weather icons
    public Sprite sunIcon, rainIcon, snowIcon, windIcon; // References to the weather icon Sprites
    public Image weatherFilterPanel; 
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
    public int poolSize = 32;
    private int currentProjectileIndex = 0;
    public GameObject lightningPrefab; // Assign this in the Inspector
    private bool _lightningCoroutineRunning = false;
    private float lastWindForceTime = 0f;
    private float windForceInterval = 2f; // Adjust the interval as needed
    private Vector2 windDirection = Vector2.zero;
    private bool isWindActive = false;
    public PhysicsMaterial2D icyMaterial;
    public GameObject iceShardPrefab; // Assign this in the Inspector
    private bool _iceShardCoroutineRunning = false;
    public GameObject icyGroundPrefab; // Assign this in the Inspector
    private GameObject icyGroundInstance;
    public GameObject orbPrefab; // Assign in Inspector
    private List<GameObject> orbs = new List<GameObject>();
    private bool areOrbsActive = false;
    private float minOrbitRadius = 2f; // Minimum orbit radius
    private float maxOrbitRadius = 5f; // Maximum orbit radius
    private float expansionSpeed = 2f; // Speed of expansion and contraction
    public GameObject cloudPrefab; 
    private GameObject cloudInstance;
    public GameObject puddlePrefab; // Assign this in the Inspector
    private List<GameObject> puddles = new List<GameObject>();
    public Vector2 spawnAreaMin = new Vector2(-5, -5); // Bottom-left corner of the spawn area
    public Vector2 spawnAreaMax = new Vector2(5, 5); // Top-right corner of the spawn area


    void Start()
    {    

        InitializeProjectilePool();
        InitializeOrbs();
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
            if (cloudInstance == null)
            {
                // Instantiate the cloud above the player with a slight offset
                cloudInstance = Instantiate(cloudPrefab, player.position + new Vector3(0, 3, 0), Quaternion.identity);
            }
            else
            {
                // Follow the player with the same offset as when instantiated
                cloudInstance.transform.position = Vector3.MoveTowards(cloudInstance.transform.position, player.position + new Vector3(0, 3, 0), Time.deltaTime * 2f);
            }

            if (currentHealth > 0)
            {
                HealWhale(); // Call a method to heal the whale
            }
        }

        else
        {
            if (cloudInstance != null)
            {
                Destroy(cloudInstance);
            }
        }

        if (currentWeather == WeatherType.Rain && !_lightningCoroutineRunning)
        {
            StartCoroutine(LightningStrikes());
        }

        else if ((currentWeather != WeatherType.Rain) && _lightningCoroutineRunning)
        {
            StopCoroutine(LightningStrikes());
            _lightningCoroutineRunning = false;
        }


        if (currentWeather == WeatherType.Sun && Time.time > lastAttackTime + attackDelay)
        {
            if (currentWeather == WeatherType.Sun)
            {
                if (Time.time > lastAttackTime + attackDelay)
                {
                    StartCoroutine(ShootFireBalls());
                    lastAttackTime = Time.time; 
                }
            }
            
        }


        if (currentWeather == WeatherType.Wind)
        {
            ApplyWindForceToPlayer();
        }

        if (currentWeather == WeatherType.Wind && !areOrbsActive)
        {
            StartCoroutine(ActivateOrbs());
            areOrbsActive = true;
        }
        else if (currentWeather != WeatherType.Wind && areOrbsActive)
        {
            StopCoroutine(ActivateOrbs());
            DeactivateOrbs();
            areOrbsActive = false;
        }


        if (currentWeather == WeatherType.Snow)
        {
            ApplyIcyConditions();
        }
        else
        {
            RemoveIcyConditions();
        }

        if (currentWeather == WeatherType.Snow && !_iceShardCoroutineRunning)
        {
            StartCoroutine(SpawnIceShards());
            _iceShardCoroutineRunning = true;
        }
        else if (currentWeather != WeatherType.Snow && _iceShardCoroutineRunning)
        {
            StopAllCoroutines(); 
            _iceShardCoroutineRunning = false;
        }
    }

    IEnumerator WeatherCycle()
    {
        while (true)
        {
            // Change weather every 10 seconds
            yield return new WaitForSeconds(10f);
            Debug.Log("Changing weather...");
            ChangeWeather();
        }
    }

    void ChangeWeather()
    {

        // Check if the current weather is Snow and clean up ice shards
        if (currentWeather == WeatherType.Snow)
        {
            StopCoroutine(SpawnIceShards()); // Stop spawning new ice shards
            _iceShardCoroutineRunning = false;
            // Optionally, destroy all existing ice shards
            foreach (var iceShard in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (iceShard.CompareTag("IceShard")) // Make sure your ice shards have a tag you can check for
                {
                    Destroy(iceShard);
                }
            }
        }

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
            StopAndClearPuddles();
            weatherIcon.sprite = sunIcon;
            filterColor = new Color(1f, 0.64f, 0f, 0.3f);
            // No particle effect for sunny weather
            break;
        case WeatherType.Rain:
            weatherIcon.sprite = rainIcon;
            filterColor = new Color(0f, 0.5f, 1f, 0.3f);
            // Adjust spawn position to account for particle system size/shape if necessary
            currentWeatherEffect = Instantiate(rainParticleSystemPrefab, spawnPosition, Quaternion.identity);
            StartPuddles();

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

    void HealWhale()
    {
        // Heal the whale by 10 HP but do not exceed maxHealth
        currentHealth = Mathf.Min(currentHealth + 10, maxHealth);
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar
        // Optionally, reset or adjust the healing interval if needed
    }
    IEnumerator LightningStrikes()
    {
        _lightningCoroutineRunning = true;
        while (cloudInstance != null && currentWeather == WeatherType.Rain)
        {
            // Instantiate the lightning effect at the cloud's position
            Instantiate(lightningPrefab, cloudInstance.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(2f); // Wait for 2 seconds before the next strike
        }
        _lightningCoroutineRunning = false;
    }

    void StartPuddles() {
    StopAndClearPuddles(); // Ensure starting fresh
    for (int i = 0; i < 3; i++) {
        GameObject puddle = Instantiate(puddlePrefab, RandomPuddlePosition(), Quaternion.identity);
        puddles.Add(puddle);
        StartCoroutine(GrowPuddle(puddle));
    }
}

void StopAndClearPuddles() {
    //StopAllCoroutines(); // Stop growing
    foreach (var puddle in puddles) {
        Destroy(puddle);
    }
    puddles.Clear();
}

Vector3 RandomPuddlePosition() {
    float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
    float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
    return new Vector3(x, y, 0); // Assuming a 2D game; adjust z if necessary
}

IEnumerator GrowPuddle(GameObject puddle) {
    Vector3 originalScale = puddle.transform.localScale;
    Vector3 targetScale = originalScale * 3; // Example target scale, adjust as needed

    float elapsedTime = 0;
    float growDuration = 10f; // Duration of growth, adjust as needed

    while (currentWeather == WeatherType.Rain && elapsedTime < growDuration) {
        puddle.transform.localScale = Vector3.Lerp(originalScale, targetScale, (elapsedTime / growDuration));
        elapsedTime += Time.deltaTime;
        yield return null;
    }
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

            yield return new WaitForSeconds(0.05f); // This controls the delay between each fireball in a volley
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
                playerRb.AddForce(windDirection * 4f, ForceMode2D.Force); // Adjust force multiplier as needed
            }
        }
    }

    void InitializeOrbs()
    {
        orbs.Clear(); // Clear existing orbs list to reset if needed
        for (int i = 0; i < 8; i++) // Increase number of orbs to 8
        {
            GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
            orb.SetActive(false);
            orbs.Add(orb);
        }
    }


    IEnumerator ActivateOrbs()
    {
        float rotationSpeed = 50f; // Speed at which the orbs rotate around the whale
        float time = 0f;

        while (currentWeather == WeatherType.Wind)
        {
            float totalAngle = time * rotationSpeed; // Total rotation angle based on time
            float orbitRadius = Mathf.Lerp(minOrbitRadius, maxOrbitRadius, (Mathf.Sin(time * expansionSpeed) + 1) / 2); // Oscillate radius

            for (int i = 0; i < orbs.Count; i++)
            {
                float angleStep = 360f / orbs.Count;
                float angle = totalAngle + i * angleStep; // Calculate the angle for each orb, including rotation
                angle *= Mathf.Deg2Rad; // Convert to radians for Mathf.Cos and Mathf.Sin

                Vector3 orbPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * orbitRadius;
                orbs[i].SetActive(true);
                orbs[i].transform.position = transform.position + orbPosition;

                // Calculate rotation so that orb is vertical on the sides and horizontal on the top and bottom
                float rotationAngle = Mathf.Rad2Deg * angle - 90; // Adjust rotation based on orbit position
                orbs[i].transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
            }

            time += Time.deltaTime;
            yield return null;
        }

        DeactivateOrbs(); // Ensure orbs are deactivated when coroutine stops
    }



    Vector3 CalculateOrbPosition(float angle, float radius)
    {
        // Calculate position based on the current angle and radius
        return transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0);
    }

    void DeactivateOrbs()
    {
        foreach (GameObject orb in orbs)
        {
            orb.SetActive(false);
        }
    }

    void ApplyIcyConditions()
    {
        Debug.Log("Applying Icy Conditions");
        if (icyGroundInstance == null) // Ensure only one instance is created
        {
            // Define the middle of the area or a fixed position for the ice rectangle
            // This could be a predefined Vector3 position or calculated based on the level layout
            // For example, setting it to the center of the camera view or a specific location
            Vector3 icyGroundPosition = CalculateIcyGroundPosition();

            // Instantiate the icy ground at the calculated position
            icyGroundInstance = Instantiate(icyGroundPrefab, icyGroundPosition, Quaternion.identity);
        }
    }

    Vector3 CalculateIcyGroundPosition()
    {
        // Example: Place the ice rectangle in the center of the camera's viewport
        // This assumes your game is 2D and uses an orthographic camera
        if (Camera.main != null)
        {
            Vector3 centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            centerPoint.z = 0; // Ensure the z position is set correctly for your game's depth usage
            return centerPoint;
        }

        // Fallback position if no camera is found
        return new Vector3(0, 0, 0);
    }

    void RemoveIcyConditions()
    {
        Debug.Log("Removing Icy Conditions");
        // Destroy the icy ground instance
        if (icyGroundInstance != null)
        {
            Destroy(icyGroundInstance);
            icyGroundInstance = null;
        }
    }



    IEnumerator SpawnIceShards()
    {
        while (currentWeather == WeatherType.Snow)
        {
            GameObject iceShard = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
            iceShard.tag = "IceShard"; // Ensure this matches the tag you created
            StartCoroutine(MoveIceShardToPlayer(iceShard));
            yield return new WaitForSeconds(2f);
        }
    }


    IEnumerator MoveIceShardToPlayer(GameObject iceShard)
    {
        Vector3 start = iceShard.transform.position;
        Vector3 end = player.position;

        while (iceShard != null && Vector3.Distance(iceShard.transform.position, end) > 0.1f)
        {
            iceShard.transform.position = Vector3.MoveTowards(iceShard.transform.position, end, Time.deltaTime * 5f); // Adjust speed as needed
            yield return null;
        }

        if (iceShard != null)
        {
            iceShard.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Stop the shard
        }
    }


void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().ExitIce();
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
