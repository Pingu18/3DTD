using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Heal : MonoBehaviour
{
    private TowerObject towerObj;

    private List<GameObject> structures = new List<GameObject>(); // list of structures in radius

    public GameObject healFX;

    private float healAmount;
    private float healRate;
    private float nextHeal;

    void Start()
    {
        towerObj = GetComponent<TowerObject>();

        healAmount = towerObj.getHeal();
        healRate = towerObj.getHealRate();

        nextHeal = 0.0f;
    }

    void Update()
    {
        HealCycle();
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
    }

    private void HealCycle()
    {
        if (Time.time > nextHeal)
        {
            for (int i = 0; i < structures.Count; i++)
            {
                TowerObject selectedTower;

                if (structures[i] != null)
                {
                    selectedTower = structures[i].GetComponent<TowerObject>();

                    if (selectedTower.getCurrentHP() < selectedTower.getMaxHP())
                    {
                        GameObject heal = Instantiate(healFX, structures[i].transform.position, Quaternion.identity);
                        heal.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                        Destroy(heal, 1.0f);
                        selectedTower.AddHP(healAmount);
                    }
                }
            }
            nextHeal = Time.time + (1 / healRate);
        }
    }
}
