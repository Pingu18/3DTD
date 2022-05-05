using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    private TowerObject towerObj;

    private float slowPercent;
    private float slowDuration;

    private int slowPercentLevel;
    private int slowDurationLevel;

    private void Start()
    {
        towerObj = GetComponent<TowerObject>();

        slowPercent = towerObj.getSlowPercent();
        slowDuration = towerObj.getSlowDuration();
    }

    // Methods
    public void applySlow(GameObject enemy)
    {
        EnemyNavMesh enemyNavMesh = enemy.GetComponent<EnemyNavMesh>();
        
        float newSpeed = enemy.GetComponent<EnemyObject>().getMoveSpeed() * (1 - (slowPercent / 100));

        if (enemyNavMesh.compareSpeed(newSpeed))
            StartCoroutine(enemyNavMesh.applySlow(newSpeed, slowDuration));
    }

    // Setter methods
    public void setSlowPercent(float newPercent)
    {
        slowPercent = newPercent;
    }

    public void setSlowDuration(float newDuration)
    {
        slowDuration = newDuration;
    }

    public void setSlowPercentLevel(int newLevel)
    {
        slowPercentLevel = newLevel;
    }

    public void setSlowDurationLevel(int newLevel)
    {
        slowDurationLevel = newLevel;
    }

    // Getter methods
    public float getSlowPercent()
    {
        return slowPercent;
    }

    public float getSlowDuration()
    {
        return slowDuration;
    }

    public int getSlowPercentLevel()
    {
        return slowPercentLevel;
    }

    public int getSlowDurationLevel()
    {
        return slowDurationLevel;
    }
}
