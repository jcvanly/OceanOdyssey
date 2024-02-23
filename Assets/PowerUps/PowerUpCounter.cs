using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpCounter : MonoBehaviour
{
    // Singleton instance
    public static PowerUpCounter instance;

    // Dictionary to store counters for each type of powerup
    private Dictionary<string, int> powerUpCounters = new Dictionary<string, int>();

    // Text objects to display counters for each type of powerup
    private TMP_Text rangeUpCounterText;
    private TMP_Text speedUpCounterText;
    private TMP_Text shootCoolDownCounterText;
    private TMP_Text healthUpCounterText;
    private TMP_Text netSpeedUpCounterText;
    private TMP_Text permHealthUpCounterText;

    private int RangeUp;
    private int HealthUp;
    private int SpeedUp;
    private int ShootCoolDown;
    private int NetSpeedUp;
    private int PermHealthUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps the object alive between scenes
        }
        else
        {
            Destroy(gameObject); // Ensures that only one instance exists
            return;
        }

        // Find and assign the text objects by tag
        rangeUpCounterText = GameObject.FindGameObjectWithTag("RangeUpCounter").GetComponent<TMP_Text>();
        speedUpCounterText = GameObject.FindGameObjectWithTag("SpeedUpCounter").GetComponent<TMP_Text>();
        shootCoolDownCounterText = GameObject.FindGameObjectWithTag("ShootCoolDownCounter").GetComponent<TMP_Text>();
        healthUpCounterText = GameObject.FindGameObjectWithTag("HealthUpCounter").GetComponent<TMP_Text>();
        netSpeedUpCounterText = GameObject.FindGameObjectWithTag("NetSpeedUpCounter").GetComponent<TMP_Text>();
        permHealthUpCounterText = GameObject.FindGameObjectWithTag("PermHealthUpCounter").GetComponent<TMP_Text>();
    }

    void Start()
    {
        powerUpCounters["RangeUp"] = RangeUp;
        powerUpCounters["SpeedUp"] = SpeedUp;
        powerUpCounters["ShootCoolDown"] = ShootCoolDown;
        powerUpCounters["HealthUp"] = HealthUp;
        powerUpCounters["NetSpeedUp"] = NetSpeedUp;
        powerUpCounters["PermHealthUp"] = PermHealthUp;
        UpdateCountersText("RangeUp");
        UpdateCountersText("SpeedUp");
        UpdateCountersText("ShootCoolDown");
        UpdateCountersText("HealthUp");
        UpdateCountersText("NetSpeedUp");
        UpdateCountersText("PermHealthUp");
    }

    public void IncreaseCounter(string powerUpType)
    {
        if (powerUpCounters.ContainsKey(powerUpType))
        {
            powerUpCounters[powerUpType]++;
            UpdateCountersText(powerUpType);
            if (powerUpType == "RangeUp")
            {
                RangeUp++;
            }
            else if (powerUpType == "SpeedUp")
            {
                SpeedUp++;
            }
            else if (powerUpType == "HealthUp")
            {
                HealthUp++;
            }
            else if (powerUpType == "NetSpeedUp")
            {
                NetSpeedUp++;
            }
            else if (powerUpType == "ShootCoolDown")
            {
                ShootCoolDown++;
            }
            else if (powerUpType == "PermHealthUp")
            {
                PermHealthUp++;
            }
        }
    }

    void UpdateCountersText(string powerUpType)
    {
        if (powerUpType == "RangeUp")
        {
            rangeUpCounterText.text = powerUpCounters["RangeUp"].ToString();
        }
        else if (powerUpType == "SpeedUp")
        {
            speedUpCounterText.text = powerUpCounters["SpeedUp"].ToString();
        }
        else if (powerUpType == "HealthUp")
        {
            healthUpCounterText.text = powerUpCounters["HealthUp"].ToString();
        }
        else if (powerUpType == "NetSpeedUp")
        {
            netSpeedUpCounterText.text = powerUpCounters["NetSpeedUp"].ToString();
        }
        else if (powerUpType == "ShootCoolDown")
        {
            shootCoolDownCounterText.text = powerUpCounters["ShootCoolDown"].ToString();
        }
        else if (powerUpType == "PermHealthUp")
        {
            permHealthUpCounterText.text = powerUpCounters["PermHealthUp"].ToString();
        }
    }
}
