using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats enemyStats;

    private string name;
    private int maxHP;
    private int currHP;
    private float moveSpeed;
    private float damage;

    private void Start()
    {
        name = enemyStats.name;
        maxHP = enemyStats.maxHealth;
        currHP = enemyStats.maxHealth;
    }

    public void takeDamage(int dmgTaken)
    {
        currHP -= dmgTaken;
        checkDeath();
    }

    private void checkDeath()
    {
        if (currHP <= 0)
            Destroy(gameObject);
    }

    public void setMaxHP(int hp)
    {
        maxHP = hp;
    }

    public void setCurrHP(int hp)
    {
        currHP = hp;
    }

    public void setMoveSpeed(float ms)
    {
        moveSpeed = ms;
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }

    public string getName()
    {
        return name;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public int getCurrHP()
    {
        return currHP;
    }

    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public float getDamage()
    {
        return damage;
    }
}
