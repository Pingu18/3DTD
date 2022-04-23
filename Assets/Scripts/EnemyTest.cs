using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float hp;

    public void TakeDamage(float dmg, GameObject tower)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            tower.GetComponent<TowerController>().RemoveTarget(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
