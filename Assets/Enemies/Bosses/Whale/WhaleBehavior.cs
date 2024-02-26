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
    public Vector2 spawnAreaMin = new Vector2(-5, -3); // Bottom-left corner of the spawn area
    public Vector2 spawnAreaMax = new Vector2(5, 3); // Top-right corner of the spawn area
    public Material heatWaveMaterial; // Add this line to reference the HeatWave material
    public GameObject whalePrefab; // Prefab of the whale to create mirages
    private List<GameObject> mirageWhales = new List<GameObject>(); // List to keep track of mirage whales
    private bool isMirageActive = false; // Flag to indicate if mirage is active
    public Transform[] spawnPoints; // Assign in the Inspector, ensure this matches the desired positions
    private Vector3 originalPosition;




    void Start()
    {    
        originalPosition = transform.position; // Store the original position
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

                if(isMirageActive == false)
                {CreateMirageWhales();
                isMirageActive = true;
                }
                
                
            }
            
        }


        if (currentWeather == WeatherType.Wind)
        {
            DestroyMirageWhales();
            isMirageActive = false;
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
            ApplyHeatWaveMaterial();
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
            StartCoroutine(MoveWhaleToOriginalPosition());
            RevertToOriginalMaterial();
            weatherIcon.sprite = windIcon;
            filterColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            
            // Consider adding a wind effect or leave it as is for wind
            break;
    }

    weatherFilterPanel.color = filterColor; // Update the panel's color
}
void ApplyHeatWaveMaterial()
    {
        Renderer whaleRenderer = GetComponent<Renderer>(); // Get the Renderer component of the whale
        if (whaleRenderer != null)
        {
            whaleRenderer.material = heatWaveMaterial; // Apply the HeatWave material
        }
    }

    void RevertToOriginalMaterial()
    {
        // Assuming you have a reference to the original material
        Renderer whaleRenderer = GetComponent<Renderer>();
        if (whaleRenderer != null)
        {
            Material defaultMaterial = new Material(Shader.Find("Sprites/Default"));
            whaleRenderer.material = defaultMaterial;
        }
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
        currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
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

            yield return new WaitForSeconds(.25f); // Wait for 2 seconds before the next strike
            yield return new WaitForSeconds(1.25f);
        }
        _lightningCoroutineRunning = false;
    }

void StartPuddles() {
    StopAndClearPuddles(); // Ensure starting fresh

    // Define corner positions for puddles
    Vector3[] cornerPositions = new Vector3[]
    {
        new Vector3(spawnAreaMin.x-3, spawnAreaMin.y, -5), // Bottom-left corner
        new Vector3(spawnAreaMax.x+3, spawnAreaMin.y, -5), // Bottom-right corner
        new Vector3(spawnAreaMin.x -3, spawnAreaMax.y, + 10), // Top-left corner
        new Vector3(spawnAreaMax.x + 3, spawnAreaMax.y, + 10), // Top-right corner
    };

    // Spawn puddles at each corner
    foreach (var position in cornerPositions)
    {
        SpawnPuddleAtPosition(position);
    }

    // Spawn a puddle directly under the whale
    SpawnPuddleAtPosition(transform.position);
}

void SpawnPuddleAtPosition(Vector3 position) {
    GameObject puddle = Instantiate(puddlePrefab, position, Quaternion.identity);
    puddles.Add(puddle);
    StartCoroutine(GrowPuddle(puddle));
}

void StopAndClearPuddles() {
    //StopAllCoroutines(); // Stop growing
    foreach (var puddle in puddles) {
        Destroy(puddle);
    }
    puddles.Clear();
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
void CreateMirageWhales()
{
    Vector3 centerPosition = transform.position; // Center position where all whales start
    
    // Determine the random index for the real whale to move to one of the spawn points
    int realWhaleIndex = Random.Range(0, spawnPoints.Length);

    List<int> usedIndices = new List<int>(); // To keep track of used spawn points

    // First, move the real whale
    Vector3 targetPositionForReal = spawnPoints[realWhaleIndex].position;
    StartCoroutine(MoveWhaleToPosition(transform, targetPositionForReal)); // Move the real whale
    usedIndices.Add(realWhaleIndex); // Mark the real whale's target as used

    // Now, handle the mirages
    for (int i = 0; i < spawnPoints.Length; i++)
    {
        if (i == realWhaleIndex) continue; // Skip the spawn point used by the real whale

        GameObject mirage = Instantiate(whalePrefab, centerPosition, Quaternion.identity); // Instantiate mirage at center

        var spriteRenderer = mirage.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color mirageColor = spriteRenderer.color;
            mirageColor.a = 0.5f; // Make mirage semi-transparent
            spriteRenderer.color = mirageColor;
        }

        StartCoroutine(MoveWhaleToPosition(mirage.transform, spawnPoints[i].position)); // Move mirage to its spawn point
        mirageWhales.Add(mirage); // Add to list for tracking
    }
}

IEnumerator MoveWhaleToPosition(Transform whale, Vector3 targetPosition)
{
    float duration = 2.0f; // Duration of the movement
    float elapsed = 0f;
    Vector3 startPosition = whale.position;

    while (elapsed < duration)
    {
        whale.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
        elapsed += Time.deltaTime;
        yield return null;
    }

    whale.position = targetPosition; // Ensure it ends exactly at the target position
}

IEnumerator MoveWhaleToOriginalPosition()
{
    float duration = 2.0f; // Duration of the movement, adjust as needed
    Vector3 startPosition = transform.position;
    float elapsed = 0f;

    while (elapsed < duration)
    {
        transform.position = Vector3.Lerp(startPosition, originalPosition, elapsed / duration);
        elapsed += Time.deltaTime;
        yield return null;
    }

    transform.position = originalPosition; // Ensure it ends exactly at the original position
}



void DestroyMirageWhales()
{
    foreach (GameObject mirage in mirageWhales)
    {
        Destroy(mirage);
    }
    mirageWhales.Clear();
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
            // Instantiate the ice shard at the whale's position or a specified spawn point
            GameObject iceShard = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
            iceShard.tag = "IceShard"; // Make sure this tag exists and is assigned to the ice shard prefab

            Rigidbody2D rb = iceShard.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Calculate direction towards the player
                Vector2 directionToPlayer = (player.position - transform.position).normalized;

                // Apply velocity in the direction to the player
                rb.velocity = directionToPlayer * chargeSpeed; // Use your charge speed variable for projectile speed

                // Optionally, you can adjust the rotation of the ice shard to face the player
                float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                iceShard.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            Collider2D collider = iceShard.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.sharedMaterial = icyMaterial; // Apply your bouncy physics material to make it bounce
            }

            yield return new WaitForSeconds(2f); // Adjust the time between shots as needed
        }
    }


    IEnumerator MoveIceShardToPlayer(GameObject iceShard)
    {
        Vector3 start = iceShard.transform.position;
        Vector3 end = player.position;
        
        while (iceShard != null && Vector3.Distance(iceShard.transform.position, end) > 0.1f)
        {
            iceShard.transform.position = Vector3.MoveTowards(iceShard.transform.position, end, Time.deltaTime * 5f);
            yield return null;
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
