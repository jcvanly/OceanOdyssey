using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ----------- ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip ----------- ")]

    public AudioClip collectPowerUp;
    public AudioClip shootNoise;
    public AudioClip takeDamage;
    public AudioClip enemyDeath;
    public AudioClip pause;
    public AudioClip unpause;
    public AudioClip mainMenu;
    public AudioClip quit;
    public AudioClip titleScreenBackground;
    public AudioClip mainMenuStart;
    public AudioClip mainMenuCredit;

    // Floor 1
    public AudioClip background;
    // Floor 2
    public AudioClip background2;
    // Floor 3
    public AudioClip background3;
    // Credits
    public AudioClip creditsMusic;
    // TitleScreen Music
    public AudioClip titleScreenMusic;
    // Bosses
    public AudioClip whaleBossMusic;
    public AudioClip kingQueenBossMusic;
    public AudioClip krakenBossMusic;
    // Dictionary to map scene names to their respective background music
    private Dictionary<string, AudioClip> sceneBackgroundMusic = new Dictionary<string, AudioClip>();

    private static AudioManager instance;

    private void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // If not, set the instance to this
            instance = this;
            // Ensure this instance persists across scenes
            DontDestroyOnLoad(gameObject);

            // Populate the sceneBackgroundMusic dictionary with scene names and their respective background music
            sceneBackgroundMusic.Add("StartArea", background);
            sceneBackgroundMusic.Add("Tide Pool Start", background2);
            sceneBackgroundMusic.Add("Stony Shore Start", background3);
            sceneBackgroundMusic.Add("TitleScreen", titleScreenMusic);
            sceneBackgroundMusic.Add("TitleScreenScene", titleScreenBackground);
            // Credits
            sceneBackgroundMusic.Add("Credits", creditsMusic);
            //Bosses
            sceneBackgroundMusic.Add("Whale", whaleBossMusic);
            sceneBackgroundMusic.Add("King & Queen", kingQueenBossMusic);
            sceneBackgroundMusic.Add("SquidArena", kingQueenBossMusic);
            // Subscribe to the scene loaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene exists in the sceneBackgroundMusic dictionary
        if (sceneBackgroundMusic.ContainsKey(scene.name))
        {
            // Set the background music for the loaded scene
            musicSource.clip = sceneBackgroundMusic[scene.name];
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
