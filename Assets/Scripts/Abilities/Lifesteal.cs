using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifesteal : MonoBehaviour
{
    // Can apply script to towers/enemies
    // If lifestealBuff enabled, give Lifesteal to nearby allies
    // If lifestealBuff disabled, give Lifesteal to self

    private List<GameObject> targets = new List<GameObject>(); // list of targets in radius

    [SerializeField] private bool lifestealBuff;
    private bool upgradeUnlocked;

    [SerializeField] public float lifestealAmount = 0.1f;

    private void Start()
    {
        upgradeUnlocked = false;

        if (!lifestealBuff)
        {
            if (GetComponent<TowerObject>())
            {
                TowerBuffHandler towerBuffHandler = GetComponent<TowerBuffHandler>();
                towerBuffHandler.enableLifesteal();

                if (towerBuffHandler.compareLifesteal(lifestealAmount))
                    towerBuffHandler.setLifestealStrength(lifestealAmount);
            } else if (GetComponent<EnemyObject>())
            {
                //GetComponent<TowerBuffHandler>().enableLifesteal();
            }
        }
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
            TowerBuffHandler towerBuffHandler = targets[i].GetComponent<TowerBuffHandler>();
            towerBuffHandler.enableLifesteal();

            if (towerBuffHandler.compareLifesteal(lifestealAmount))
               towerBuffHandler.setLifestealStrength(lifestealAmount);
        }
    }

    public void addTarget(GameObject target)
    {
        targets.Add(target);

        if (upgradeUnlocked)
        {
            TowerBuffHandler towerBuffHandler = target.GetComponent<TowerBuffHandler>();
            towerBuffHandler.enableLifesteal();

            if (towerBuffHandler.compareLifesteal(lifestealAmount))
                towerBuffHandler.setLifestealStrength(lifestealAmount);
        }
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);

        if (upgradeUnlocked)
        {
            target.GetComponent<TowerBuffHandler>().disableLifesteal();
            target.GetComponent<TowerBuffHandler>().setLifestealStrength(0f);
        }
    }

    public bool getLifestealBuff()
    {
        return lifestealBuff;
    }
}
