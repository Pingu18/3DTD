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

    private void setSlowPercent(float newPercent)
    {
        slowPercent = newPercent;
    }

    private void setSlowDuration(float newDuration)
    {
        slowDuration = newDuration;
    }

    public void applySlow(GameObject enemy)
    {
        float newSpeed = enemy.GetComponent<EnemyObject>().getMoveSpeed() * (1 - (slowPercent / 100));

        StartCoroutine(enemy.GetComponent<EnemyNavMesh>().applySlow(newSpeed, slowDuration));
    }
}
