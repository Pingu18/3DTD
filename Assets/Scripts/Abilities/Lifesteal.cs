using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : MonoBehaviour
{
    // Can apply script to towers/enemies
    // If lifestealBuff enabled, give Lifesteal to nearby allies
    // If lifestealBuff disabled, give Lifesteal to self

    private TowerObject towerObj;

    private List<GameObject> targets = new List<GameObject>(); // list of targets in radius

    [SerializeField] private bool lifestealBuff;
    private bool upgradeUnlocked;

    [SerializeField] public float lifestealPercent = 10f;

    private void Start()
    {
        if (GetComponent<TowerObject>())
            towerObj = GetComponent<TowerObject>();

        upgradeUnlocked = false;

        if (!lifestealBuff)
        {
            if (towerObj)
            {
                TowerBuffHandler towerBuffHandler = GetComponent<TowerBuffHandler>();
                towerBuffHandler.enableLifesteal();

                if (towerBuffHandler.compareLifesteal(lifestealPercent))
                    towerBuffHandler.setLifestealPercent(lifestealPercent);
            } else if (GetComponent<EnemyObject>())
            {
                //GetComponent<TowerBuffHandler>().enableLifesteal();
            }
        }
    }

    public void updateLifestealAmount(float newLifesteal)
    {
        lifestealPercent = newLifesteal;

        if (lifestealBuff)
            applyToTargets();
    }

    public void enableUpgrade()
    {
        upgradeUnlocked = true;

        applyToTargets();
    }

    private void applyToTargets()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                TowerBuffHandler towerBuffHandler = targets[i].GetComponent<TowerBuffHandler>();
                towerBuffHandler.enableLifesteal();

                if (towerBuffHandler.compareLifesteal(lifestealPercent))
                    towerBuffHandler.setLifestealPercent(lifestealPercent);
            }
        }
    }

    public void addTarget(GameObject target)
    {
        targets.Add(target);

        if (upgradeUnlocked)
        {
            TowerBuffHandler towerBuffHandler = target.GetComponent<TowerBuffHandler>();
            towerBuffHandler.enableLifesteal();

            if (towerBuffHandler.compareLifesteal(lifestealPercent))
                towerBuffHandler.setLifestealPercent(lifestealPercent);
        }
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);

        if (upgradeUnlocked)
        {
            target.GetComponent<TowerBuffHandler>().disableLifesteal();
            target.GetComponent<TowerBuffHandler>().setLifestealPercent(0f);
        }
    }

    public bool getLifestealBuff()
    {
        return lifestealBuff;
    }
}
