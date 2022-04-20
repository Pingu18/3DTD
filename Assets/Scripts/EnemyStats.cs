using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    private GameObject gameObj;

    private string name;
    private float maxHP;
    private float currHP;
    private float moveSpeed;
    private float damage;

    public EnemyStats(GameObject obj, float hp, float ms, float dmg)
    {
        name = "CommonEnemy";

        gameObj = obj;
        maxHP = hp;
        currHP = hp;
        moveSpeed = ms;
        damage = dmg;
    }

    public void takeDamage(float dmgTaken)
    {
        currHP = currHP - dmgTaken;
    }

    public void setMaxHP(float hp)
    {
        maxHP = hp;
    }
    
    public void setCurrHP(float hp)
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

    public float getMaxHP()
    {
        return maxHP;
    }
    
    public float getCurrHP()
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
