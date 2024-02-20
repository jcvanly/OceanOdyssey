using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PowerUpCounter : MonoBehaviour
{
    // Dictionary to store counters for each type of powerup
    private Dictionary<string, int> powerUpCounters = new Dictionary<string, int>();

    // Text objects to display counters for each type of powerup
    public TMP_Text rangeUpCounterText;
    public TMP_Text speedUpCounterText;
    public TMP_Text ShootCoolDownCounterText;
    public TMP_Text HealthUpCounterText;
    public TMP_Text NetSpeedUpCounterText;
    public TMP_Text PermHealthUpCounterText;

    public static PowerUpCounter instance;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        powerUpCounters["RangeUp"] = 0;
        powerUpCounters["SpeedUp"] = 0;
        powerUpCounters["ShootCoolDown"] = 0;
        powerUpCounters["HealthUp"] = 0;
        powerUpCounters["NetSpeedUp"] = 0;
        powerUpCounters["PermHealthUp"] = 0;

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
            HealthUpCounterText.text = powerUpCounters["HealthUp"].ToString();
        }
        else if (powerUpType == "NetSpeedUp")
        {
            NetSpeedUpCounterText.text = powerUpCounters["NetSpeedUp"].ToString();
        }
        else if (powerUpType == "ShootCoolDown")
        {
            ShootCoolDownCounterText.text = powerUpCounters["ShootCoolDown"].ToString();
        }
        else if (powerUpType == "PermHealthUp")
        {
            PermHealthUpCounterText.text = powerUpCounters["PermHealthUp"].ToString();
        }
    }
}