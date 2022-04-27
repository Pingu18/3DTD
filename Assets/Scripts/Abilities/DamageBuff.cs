using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBuff : MonoBehaviour
{
    private TowerObject towerObj;

    [SerializeField] private List<GameObject> structures = new List<GameObject>(); // list of structures in radius

    void Start()
    {
        towerObj = GetComponent<TowerObject>();
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
        // apply buff
        structure.GetComponent<TowerObject>().increaseDamage(0.1f);
    }

    public void RemoveStructure(GameObject structure)
    {
        structures.Remove(structure);
        // remove buff
        structure.GetComponent<TowerObject>().reduceDamage(0.1f);
    }
}
