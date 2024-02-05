using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/NetSpeedBuff")]
public class NetSpeedBuff : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Shooting>().bulletSpeed += amount;

    }
}