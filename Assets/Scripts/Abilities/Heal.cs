using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Heal : MonoBehaviour
{
    private TowerObject towerObj;
    private ActionDict actionDict;

    private List<GameObject> structures = new List<GameObject>(); // list of structures in radius

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
        towerObj = GetComponent<TowerObject>();
        actionDict = transform.parent.parent.gameObject.GetComponent<ActionDict>();

        baseHealAmount = towerObj.getHeal();
        baseHealRate = towerObj.getHealRate();

        healAmount = baseHealAmount;
        healRate = baseHealRate;

        healLvl = 0;
        healRateLvl = 0;

        nextHeal = 0.0f;

        StartCoroutine(startHealCycle());
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
    }

    private IEnumerator startHealCycle()
    {
        while (keepHealing)
        {
            yield return StartCoroutine(readyToHeal());
            healStructures();
        }
    }

    private IEnumerator readyToHeal()
    {
        yield return new WaitUntil(() => structures.Count > 0 && Time.time > nextHeal);
    }

    private void healStructures()
    {
        for (int i = 0; i < structures.Count; i++)
        {
            TowerObject selectedTower;

            if (structures[i] != null)
            {
                selectedTower = structures[i].GetComponent<TowerObject>();

                if (selectedTower.getCurrentHP() < selectedTower.getMaxHP())
                {
                    GameObject heal = Instantiate(actionDict.getHealFX(towerObj.getName()), structures[i].transform.position, Quaternion.identity);
                    heal.transform.GetComponentInChildren<VisualEffect>().Play();
                    Destroy(heal, 1.0f);
                    selectedTower.AddHP(healAmount);
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
