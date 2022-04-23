using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyStats enemyStats;
    private EnemyController enemyController;

    private string name;
    private float maxHP;
    private float currHP;
    private float moveSpeed;
    private float damage;

    private void Start()
    {
        name = enemyStats.name;
        maxHP = enemyStats.maxHealth;
        currHP = enemyStats.maxHealth;

        enemyController = transform.parent.GetComponent<EnemyController>();
    }

    public void takeDamage(float dmgTaken, GameObject tower)
    {
        currHP -= dmgTaken;
        checkDeath(tower);
    }

    private void checkDeath(GameObject tower)
    {
        if (currHP <= 0)
        {
            tower.GetComponent<TowerController>().RemoveTargetOnDeath(this.gameObject);
            enemyController.decrementEnemiesAlive();
            Destroy(gameObject);
        }

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
