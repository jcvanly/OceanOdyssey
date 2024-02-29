    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class WhaleBehavior : MonoBehaviour
    {
        private float lastLavaSpawnTime = 0f;
        public float lavaSpawnInterval = 2f; // Time between lava spawns    
        public GameObject lavaTilePrefab; 

        public GameObject iceRockPrefab; 
        private List<GameObject> iceRocks = new List<GameObject>();
        private bool areIceRocksActive = false;
        //private float iceRockOrbitRadius = 10f;
        private bool shootDiagonal = true;
        public GameObject fireBall;
        public float shootInterval = 2f;
        private float shootTimer;
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
        public float iceChargeSpeed = 2f;
        public float attackDelay = 2f; // Time between attacks
        public PlayerHealth playerHealth;
        public Transform player; // Reference to the player's transform
        private Rigidbody2D rb;
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
        private float maxOrbitRadius = 7f; // Maximum orbit radius
        private float expansionSpeed = 2f; // Speed of expansion and contraction
        public GameObject cloudPrefab; 
        private GameObject cloudInstance;
        public GameObject puddlePrefab; // Assign this in the Inspector
        private List<GameObject> puddles = new List<GameObject>();
        public Vector2 spawnAreaMin = new Vector2(-100, -3); // Bottom-left corner of the spawn area
        public Vector2 spawnAreaMax = new Vector2(100, 3); // Top-right corner of the spawn area
        public Material heatWaveMaterial; // Add this line to reference the HeatWave material
        public GameObject whalePrefab; // Prefab of the whale to create mirages
        private List<GameObject> mirageWhales = new List<GameObject>(); // List to keep track of mirage whales
        private bool isMirageActive = false; // Flag to indicate if mirage is active
        public Transform[] spawnPoints; 
        private Vector3 originalPosition;
        public int amountHealed = 0;
        private float healRate = 1.0f; // Time in seconds between each heal
        private float lastHealTime = 0;
        public Sprite normalSprite; 
        public Sprite enragedSprite; 
        public bool isEnraged = false; // Tracks whether the whale is enraged
        public GameObject tornadoPrefab; // Assign this in the Unity Inspector
        private float tornadoShootTimer = 3f; // Time between tornado shots when enraged and raining
        private float lastTornadoShotTime = 0f;
        public GameObject waterOrbPrefab; // Assign this in Inspector
        private bool _waterOrbCoroutineRunning = false; // Add this flag to the class variables


        void Start()
        {        
            InitializeIceRocks();
            shootTimer = shootInterval;
            originalPosition = transform.position; // Store the original position
            InitializeOrbs();
            HideVictoryScreen();
            currentHealth = maxHealth;
            currentWeather = WeatherType.Rain;
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
                    // Instantiate the cloud above the whale
                    cloudInstance = Instantiate(cloudPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
                }
                else
                {
                    // Follow the player with the same offset as when instantiated
                    cloudInstance.transform.position = Vector3.MoveTowards(cloudInstance.transform.position, player.position + new Vector3(0, 3, 0), Time.deltaTime * 2f);
                }

                if (currentWeather == WeatherType.Rain && Time.time - lastHealTime >= healRate && amountHealed < 10)
                {
                    HealWhale();
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

            if (isEnraged && currentWeather == WeatherType.Rain)
                {
                    // Check if it's time to shoot a tornado
                    if (Time.time > lastTornadoShotTime + tornadoShootTimer)
                    {
                        ShootTornadoAtPlayer();
                        lastTornadoShotTime = Time.time; // Reset the timer
                    }
                }

            else if ((currentWeather != WeatherType.Rain) && _lightningCoroutineRunning)
            {
                StopCoroutine(LightningStrikes());
                _lightningCoroutineRunning = false;
            }
            
            if (isEnraged && currentWeather == WeatherType.Sun)
            {
                if (!_waterOrbCoroutineRunning)
                {
                    StartCoroutine(ShootWaterOrbAtPlayerCoroutine());
                    _waterOrbCoroutineRunning = true;
                }
            }
            else if (_waterOrbCoroutineRunning)
            {
                StopCoroutine(ShootWaterOrbAtPlayerCoroutine());
                _waterOrbCoroutineRunning = false;
            }


            if (currentWeather == WeatherType.Sun)
            {

                shootTimer -= Time.deltaTime;

                // Check if shootTimer is less than or equal to 0
                if (shootTimer <= 0)
                {
                    shootFireball(); // Shoot fireball
                    // Reset the shootTimer to shootInterval
                    shootTimer = shootInterval;
                }

                if(isMirageActive == false)
                {
                    CreateMirageWhales();
                    isMirageActive = true;
                }
                
            }

            if (isEnraged && currentWeather == WeatherType.Wind)
            {
                if (!areIceRocksActive)
                {
                    StartCoroutine(ActivateIceRocks());
                    areIceRocksActive = true;
                }
            }
            else
            {
                if (areIceRocksActive)
                {
                    // This logic will now correctly deactivate ice rocks when the weather is not windy or the whale is not enraged
                    StopCoroutine(ActivateIceRocks()); // You might need to adjust this if you cannot directly stop a coroutine started with a while loop inside
                    foreach (GameObject iceRock in iceRocks)
                    {
                        iceRock.SetActive(false); // Ensure to deactivate each ice rock immediately
                    }
                    areIceRocksActive = false;
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

            if (isEnraged && currentWeather == WeatherType.Snow)
            {
                if (Time.time > lastLavaSpawnTime + 8)
                {
                    SpawnLavaTiles();
                    lastLavaSpawnTime = Time.time;
                }
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
                foreach (var iceShard in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                {
                    if (iceShard.CompareTag("IceShard"))
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

        if (currentWeatherEffect != null)
        {
            Destroy(currentWeatherEffect);
        }

        Camera mainCamera = Camera.main;
        float topOfScreen = mainCamera.orthographicSize;
        Vector3 spawnPosition = mainCamera.transform.position + new Vector3(0, topOfScreen, 0);
        spawnPosition.z = 0; 

        switch (currentWeather)
        {
            case WeatherType.Sun:
                
                StopAndClearPuddles();
                ApplyHeatWaveMaterial();
                weatherIcon.sprite = sunIcon;
                filterColor = new Color(1f, 0.64f, 0f, 0.3f);
                break;
            case WeatherType.Rain:
                weatherIcon.sprite = rainIcon;
                filterColor = new Color(0f, 0.5f, 1f, 0.3f);
                currentWeatherEffect = Instantiate(rainParticleSystemPrefab, spawnPosition, Quaternion.identity);
                StartPuddles();
                RemoveLavaTiles(); // Call a method to remove all lava tiles
                break;
            case WeatherType.Snow:
                weatherIcon.sprite = snowIcon;
                filterColor = new Color(1f, 1f, 1f, 0.3f);
                currentWeatherEffect = Instantiate(snowParticleSystemPrefab, spawnPosition, Quaternion.identity);
                StartCoroutine(MoveWhaleToOriginalPosition());

                break;
            case WeatherType.Wind:
                RevertToOriginalMaterial();
                weatherIcon.sprite = windIcon;
                filterColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
                
                break;
        }

        weatherFilterPanel.color = filterColor; // Update the panel's color
    }

    void shootFireball()
{
    float speed = 4f;
    if (shootDiagonal)
    {
        // Shooting in all diagonal directions
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * speed;
    }
    else
    {
        // Shooting in cardinal directions
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1).normalized * speed;
        Instantiate(fireBall, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1).normalized * speed;
    }

    // Toggle the firing pattern for the next call
    shootDiagonal = !shootDiagonal;
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
        void HealWhale()
        {
            
            // Assuming you want to heal up to 10 HP during the rain, spread over time
            if (currentHealth < maxHealth)
            {
                int healAmount = 1; // Heal 1 HP at a time
                currentHealth += healAmount;
                currentHealth = Mathf.Min(currentHealth, maxHealth); // Ensure don't exceed maxHealth
                amountHealed += healAmount;
                lastHealTime = Time.time; // Update the last heal time

                healthBar.UpdateHealthBar(currentHealth, maxHealth); // Update the health bar
                Debug.Log("current Health is: " + currentHealth);
            }

            // Reset amountHealed when not raining or after reaching the heal cap
            if (currentWeather != WeatherType.Rain || amountHealed >= 10)
            {
                amountHealed = 0;
            }
            
        }

        void ShootTornadoAtPlayer()
        {
            GameObject tornado = Instantiate(tornadoPrefab, transform.position, Quaternion.identity);
        }



        IEnumerator LightningStrikes()
{
    _lightningCoroutineRunning = true;
    while (cloudInstance != null && currentWeather == WeatherType.Rain)
    {
        // Calculate the spawn position so that the top of the lightning sprite appears to come from the bottom of the cloud.
        SpriteRenderer cloudSpriteRenderer = cloudInstance.GetComponent<SpriteRenderer>();
        if (cloudSpriteRenderer != null)
        {
            Vector3 cloudBottom = cloudInstance.transform.position - new Vector3(0, cloudSpriteRenderer.bounds.extents.y, 0);
            // Instantiate the lightning prefab at the calculated position.
            GameObject lightning = Instantiate(lightningPrefab, cloudBottom, Quaternion.identity);

            // Set the parent of the lightning GameObject to the cloud instance.
            lightning.transform.SetParent(cloudInstance.transform);

            lightning.transform.localPosition = new Vector3(0, -cloudSpriteRenderer.bounds.extents.y, 0);
        }

        // Wait for the next lightning strike.
        yield return new WaitForSeconds(.25f); // Short delay before the next strike.
        yield return new WaitForSeconds(1.25f); // Longer delay for a more natural effect.
    }
    _lightningCoroutineRunning = false;
}


    void StartPuddles()
{
    StopAndClearPuddles(); // Clear existing puddles first

    int puddleCount = 15; // Number of puddles to spawn
    float minDistanceApart = 1.5f; // Minimum distance between puddles

    for (int i = 0; i < puddleCount; i++)
    {
        Vector3 randomPosition = GetRandomPositionInSpawnArea();
        bool isTooClose;

        // Attempt up to 10 times to find a suitable position that isn't too close to other puddles
        int attempts = 0;
        do
        {
            isTooClose = false;
            foreach (GameObject existingPuddle in puddles)
            {
                if (Vector3.Distance(randomPosition, existingPuddle.transform.position) < minDistanceApart)
                {
                    isTooClose = true;
                    randomPosition = GetRandomPositionInSpawnArea();
                    break; // Exit the foreach loop and try a new position
                }
            }
            attempts++;
        } while (isTooClose && attempts < 10);

        // If a suitable position is found, spawn the puddle
        if (!isTooClose)
        {
            SpawnPuddleAtPosition(randomPosition);
        }
    }
}


    Vector3 GetRandomPositionInSpawnArea()
    {
        float randomX = Random.Range(-9, 9);
        float randomY = Random.Range(-3, 3);
        return new Vector3(randomX, randomY, 0); 
    }

    void SpawnPuddleAtPosition(Vector3 position)
    {
        GameObject puddle = Instantiate(puddlePrefab, position, Quaternion.identity);
        puddles.Add(puddle);
        StartCoroutine(GrowPuddle(puddle)); // Assuming you want the puddles to grow over time
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
        Vector3 targetScale = originalScale * 3; 

        float elapsedTime = 0;
        float growDuration = 10f; // Duration of growth

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

        // handle the mirages
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i == realWhaleIndex) continue; // Skip the spawn point used by the real whale

            GameObject mirage = Instantiate(whalePrefab, centerPosition, Quaternion.identity);
            WhaleMirage mirageScript = mirage.GetComponent<WhaleMirage>();
            if (mirageScript != null)
            {
                mirageScript.mainWhale = this; // 'this' refers to the current instance of WhaleBehavior
            }

            StartCoroutine(MoveWhaleToPosition(mirage.transform, spawnPoints[i].position)); // Move mirage to its spawn point
            mirageWhales.Add(mirage); 
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
        float duration = 2.0f; // Duration of the movement
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

    IEnumerator ShootWaterOrbAtPlayerCoroutine()
    {
        while (isEnraged && currentWeather == WeatherType.Sun)
        {
            yield return new WaitForSeconds(2f);
            GameObject waterOrbInstance = Instantiate(waterOrbPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2f); // Adjust frequency of shooting as desired
        }
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float desiredDistance = 6f; // The desired distance from the player
            float moveSpeed = 2f; // Adjust whale speed
            
            Vector2 direction = (player.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > desiredDistance)
            {
                // Move only if the distance is greater than the desired distance
                Vector2 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                rb.MovePosition(newPosition);
            }
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
                MoveTowardsPlayer();

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
            orbs.Clear(); // Clear existing orbs to reset
            for (int i = 0; i < 8; i++) // Increase number of orbs to 8
            {
                GameObject orb = Instantiate(orbPrefab, transform.position, Quaternion.identity);
                orb.SetActive(false);
                orbs.Add(orb);
            }
        }
        IEnumerator ActivateOrbs()
        {
            float rotationSpeed = 15f; // Speed at which the orbs rotate around the whale
            float time = 0f;

            while (currentWeather == WeatherType.Wind)
            {
                float totalAngle = time * rotationSpeed; // Total rotation angle based on time
                // Oscillate the orbitRadius between minOrbitRadius and maxOrbitRadius
                float orbitRadius = Mathf.Lerp(minOrbitRadius, maxOrbitRadius, (Mathf.Sin(time * expansionSpeed) + 1) / 2);

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
                // Define the middle of the arena
                Vector3 icyGroundPosition = CalculateIcyGroundPosition();

                // Create the icy ground at the calculated position
                icyGroundInstance = Instantiate(icyGroundPrefab, icyGroundPosition, Quaternion.identity);
            }
        }
        Vector3 CalculateIcyGroundPosition()
        {
            if (Camera.main != null)
            {
                Vector3 centerPoint = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                centerPoint.z = 0;
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
            yield return new WaitForSeconds(2f);

            while (currentWeather == WeatherType.Snow)
            {
                GameObject iceShard = Instantiate(iceShardPrefab, transform.position, Quaternion.identity);
                iceShard.tag = "IceShard";

                Rigidbody2D rb = iceShard.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Calculate direction towards the player
                    Vector2 directionToPlayer = (player.position - transform.position).normalized;

                    // Apply velocity in the direction to the player
                    rb.velocity = directionToPlayer * iceChargeSpeed * 2f; 

                    float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
                    iceShard.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }

                Collider2D collider = iceShard.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.sharedMaterial = icyMaterial; // Apply bouncy physics material
                }

                yield return new WaitForSeconds(2f); // Adjust the time between shots as needed
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
            healthBar.UpdateHealthBar(currentHealth, maxHealth); 

            // Check if the whale is not already enraged and if health is at or below 50%
            if (!isEnraged && currentHealth <= maxHealth / 2)
            {
                BecomeEnraged();
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void BecomeEnraged()
        {
            isEnraged = true;
            // Change the whale's sprite to the enraged version
            GetComponent<SpriteRenderer>().sprite = enragedSprite;
        
            Debug.Log("The whale is now enraged!");
        }
        void Die()
        {
            
            weatherIcon.enabled = false; 
            weatherFilterPanel.enabled = false;

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

        public void HideVictoryScreen()
        {
            victoryText.SetActive(false);
            nextIslandButton.SetActive(false);
        }

        void InitializeIceRocks()
        {
            iceRocks.Clear(); // Clear existing ice rocks to reset
            for (int i = 0; i < 8; i++) // Create 8 ice rocks
            {
                GameObject iceRock = Instantiate(iceRockPrefab, transform.position, Quaternion.identity);
                iceRock.SetActive(false); // Start inactive
                iceRocks.Add(iceRock);
            }
        }
IEnumerator ActivateIceRocks()
{
    // Assuming you still have your initial setup
    float pulseSpeed = 2f; // Speed of the pulsing effect
    float minRadius = 5f; // Minimum radius of the orbit
    float maxRadius = 15f; // Maximum radius of the orbit
    float pulseTime = 0f; // Time counter for the pulsing effect

    while (isEnraged && currentWeather == WeatherType.Wind)
    {
        float rotationSpeed = 20f; // Adjust as needed for ice rocks
        float totalAngle = Time.time * rotationSpeed;

        // Calculate the current orbit radius based on a sine wave for the pulsing effect
        float currentOrbitRadius = Mathf.Lerp(minRadius, maxRadius, (Mathf.Sin(pulseTime * pulseSpeed) + 1) / 2);

        for (int i = 0; i < iceRocks.Count; i++)
        {
            float angleStep = 360f / iceRocks.Count;
            float angle = totalAngle + i * angleStep;
            angle *= Mathf.Deg2Rad;

            Vector3 iceRockPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * currentOrbitRadius;
            iceRocks[i].SetActive(true);
            iceRocks[i].transform.position = transform.position + iceRockPosition;
        }

        pulseTime += Time.deltaTime;
        yield return null;
    }
    DeactivateIceRocks();
}

    void DeactivateIceRocks()
    {
        foreach (GameObject iceRock in iceRocks)
        {
            iceRock.SetActive(false);
        }
    }

    private void SpawnLavaTiles()
    {
        int lavaTilesToSpawn = 4; // Number of lava tiles to spawn
        float spawnRadius = 7f; // Radius around the whale within which tiles will spawn

        for (int i = 0; i < lavaTilesToSpawn; i++)
        {
            Vector2 spawnPosition = transform.position + (Vector3)(Random.insideUnitCircle * spawnRadius);
            GameObject lavaTile = Instantiate(lavaTilePrefab, spawnPosition, Quaternion.identity);
            StartCoroutine(GrowLavaTile(lavaTile)); // Start growing the lava tile
        }
    }

    IEnumerator GrowLavaTile(GameObject lavaTile)
    {
        Vector3 originalScale = lavaTile.transform.localScale; // Store the original scale
        Vector3 targetScale = originalScale * 40; // Define the target scale (e.g., 3 times the original size)

        float elapsedTime = 0;
        float growDuration = 10f; // Duration over which the tile grows

        while (elapsedTime < growDuration)
        {
            lavaTile.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / growDuration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
    }


void RemoveLavaTiles()
{
    GameObject[] lavaTiles = GameObject.FindGameObjectsWithTag("LavaTile");
    Debug.Log($"Destroying {lavaTiles.Length} lava tiles.");
    foreach (GameObject lavaTile in lavaTiles)
    {
        Destroy(lavaTile); // Destroy each lava tile
    }
}

        


    }
