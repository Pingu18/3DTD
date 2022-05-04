using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Heal : MonoBehaviour
{
    private EnemyObject enemyObj;
    private TowerObject towerObj;
    private ActionDict actionDict;

    private List<GameObject> targets = new List<GameObject>(); // list of targets in radius

    private float baseHealAmount;
    private float baseHealRate;

    private float healAmount;
    private float healRate;

    private int healLvl;
    private int healRateLvl;

    private float nextHeal;

    private bool keepHealing = true;

    void Start()
    {
        if (this.tag == "Structure")
        {
            actionDict = transform.parent.parent.gameObject.GetComponent<ActionDict>();
            towerObj = GetComponent<TowerObject>();

            baseHealAmount = towerObj.getHeal();
            baseHealRate = towerObj.getHealRate();
        } else if (this.tag == "Enemy")
        {
            enemyObj = GetComponent<EnemyObject>();

            baseHealAmount = enemyObj.getHeal();
            baseHealRate = enemyObj.getHealRate();
        }

        healAmount = baseHealAmount;
        healRate = baseHealRate;

        healLvl = 0;
        healRateLvl = 0;

        nextHeal = 0.0f;

        StartCoroutine(startHealCycle());
    }

    public void addTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void removeTarget(GameObject target)
    {
        targets.Remove(target);
    }

    private IEnumerator startHealCycle()
    {
        while (keepHealing)
        {
            yield return StartCoroutine(readyToHeal());

            if (this.tag == "Structure")
                healTargetStructure();
            else if (this.tag == "Enemy")
                healTargetEnemy();
        }
    }

    private IEnumerator readyToHeal()
    {
        yield return new WaitUntil(() => targets.Count > 0 && Time.time > nextHeal);
    }

    private void healTargetStructure()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            TowerObject selectedTower;

            if (targets[i] != null)
            {
                selectedTower = targets[i].GetComponent<TowerObject>();

                if (selectedTower.getCurrentHP() < selectedTower.getMaxHP())
                {
                    GameObject heal = Instantiate(actionDict.getHealFX(towerObj.getName()), targets[i].transform.position, Quaternion.identity);
                    heal.transform.GetComponentInChildren<VisualEffect>().Play();
                    Destroy(heal, 1.0f);
                    selectedTower.AddHP(healAmount);
                }
            }
        }

        nextHeal = Time.time + (1 / healRate);
    }

    private void healTargetEnemy()
    {
        for (int i = 0; i < targets.Count; i++)
        {   
            EnemyObject selectedEnemy;

            if (targets[i] != null)
            {
                selectedEnemy = targets[i].GetComponent<EnemyObject>();

                if (selectedEnemy.getCurrentHP() < selectedEnemy.getMaxHP())
                {
                    selectedEnemy.addHP(healAmount);
                }
            }
        }

        nextHeal = Time.time + (1 / healRate);
    }

    public void setHealLevel(int newLevel)
    {
        healLvl = newLevel;
    }

    public void setHealRateLevel(int newLevel)
    {
        healRateLvl = newLevel;
    }

    public void setBaseHealAmount(float newHeal)
    {
        baseHealAmount = newHeal;
        healAmount = newHeal;
    }

    public void setHealAmount(float newHeal)
    {
        healAmount = newHeal;
    }

    public void setBaseHealRate(float newRate)
    {
        baseHealRate = newRate;
        healRate = newRate;
    }

    public void setHealRate(float newRate)
    {
        healRate = newRate;
    }

    public int getHealAmountLevel()
    {
        return healLvl;
    }

    public int getHealRateLevel()
    {
        return healRateLvl;
    }

    public float getHealAmount()
    {
        return healAmount;
    }

    public float getHealRate()
    {
        return healRate;
    }
}
