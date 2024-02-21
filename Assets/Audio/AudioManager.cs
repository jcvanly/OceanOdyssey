using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Header("----------- Audio Source ----------- ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip ----------- ")]
    public AudioClip background;
    public AudioClip collectPowerUp;
    public AudioClip shootNoise;
    public AudioClip takeDamage;
    public AudioClip enemyDeath;
    public AudioClip pause;
    public AudioClip unpause;
    public AudioClip mainMenu;
    public AudioClip quit;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        musicSource.clip = background;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}