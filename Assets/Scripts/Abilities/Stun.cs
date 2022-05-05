using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    private TowerObject towerObj;

    [Header("Lightning Tower")]
    [SerializeField] private int attacksToStun;
    [SerializeField] private float stunDuration;
    private int currentAttack;

    [Header("Water Tower")]
    [SerializeField] private float freezeDuration;

    // To keep track of which tower this script is on (doesn't actually do anything)
    [Header("Tracking Purposes")]
    [SerializeField] private bool isWaterTower;
    [SerializeField] private bool isLightningTower;

    private void Start()
    {
        towerObj = GetComponent<TowerObject>();

        currentAttack = 0;
    }

    public void updateDuration(float newDuration)
    {
        if (towerObj.getName().Equals("Water Tower"))
            freezeDuration = newDuration;
        else if (towerObj.getName().Equals("Lightning Tower"))
            stunDuration = newDuration;
    }

    public void addAttack()
    {
        currentAttack += 1;
    }

    public void checkForFreeze(GameObject target)
    {
        EnemyBuffHandler enemyBuffHandler = target.GetComponent<EnemyBuffHandler>();

        if (enemyBuffHandler.getChillStacks() >= 3)
        {
            StartCoroutine(target.GetComponent<EnemyNavMesh>().applyStun(freezeDuration));
            enemyBuffHandler.resetChillStacks();
        }
    }

    public bool checkForStun()
    {
        if (currentAttack >= attacksToStun)
            return true;

        return false;
    }

    public void stunTarget(GameObject target)
    {
        StartCoroutine(target.GetComponent<EnemyNavMesh>().applyStun(stunDuration));
    }

    public void resetAttacks()
    {
        currentAttack = 0;
    }
}
