using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/PermHealthUp")]
public class PermHealthUp : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerHealth>().maxHealth += amount;
        target.GetComponent<PlayerHealth>().health += amount;
    }
}