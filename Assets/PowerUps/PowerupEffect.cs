using UnityEngine;
using TMPro;
public abstract class PowerupEffect : ScriptableObject
{
    public abstract void Apply(GameObject target, TextMeshProUGUI notificationText);
}