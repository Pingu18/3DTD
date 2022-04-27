using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDebuff : MonoBehaviour
{
    private TowerObject towerObj;

    private List<GameObject> targets = new List<GameObject>(); // list of enemies in radius

    void Start()
    {
        towerObj = GetComponent<TowerObject>();
    }

    public void AddTarget(GameObject target)
    {
        targets.Add(target);
        // apply debuff
        target.GetComponent<EnemyObject>().reduceDamage(0.2f);
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
        // remove debuff
        target.GetComponent<EnemyObject>().resetDamage();

    }
}
