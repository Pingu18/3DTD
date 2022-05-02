using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public string enemyName;
    public string element;
    public float damageMultiplier;

    public int maxHealth;
    public float damage;
    public float attackSpeed;
    public float range;
    public float speed;

    public float healAmount;
    public float healRate;

    public int worth;
}
