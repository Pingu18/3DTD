using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower Stats", menuName = "Tower/Stats")]
public class TowerStats : ScriptableObject
{
    [Header("Static Stats")]
    public string towerName;
    public string element;

    [Header("Basic Stats")]
    public int maxHealth;
    public float damage;
    public float attackSpeed;
    public float range;
    public float lifesteal;

    [Header("Heal Stats")]
    public float heal;
    public float healRate;

    [Header("Slow Stats")]
    public float slowPercent;
    public float slowDuration;

    [Header("Special Tier")]
    public float special;
    public string specialDesc;
    public string specialUpgradeDesc;

    [Header("Misc")]
    public float damageMultiplier;
    public float takeDamageMultiplier;
    public int cost;

    [Header("Upgrades")]
    public string[] upgrades;
}
