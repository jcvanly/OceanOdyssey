using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/DamageBuff")]
public class DamageBuff : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<Bullet>().damage += amount;
        target.GetComponent<SpriteRenderer>().color = Color.red;
    }
}