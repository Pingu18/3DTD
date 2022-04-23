using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillAttack : MonoBehaviour
{
    public GameObject parentTower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && parentTower != null)
        {
            //print(parentTower.name);
            other.gameObject.GetComponent<IDamageable>().takeDamage(parentTower.GetComponent<TowerController>().damage, parentTower);
            other.gameObject.GetComponent<EnemyNavMesh>().applySlow(0.6f);
            //other.gameObject.GetComponent<EnemyTest>().TakeDamage(parentTower.GetComponent<TowerController>().damage, parentTower);
        }
    }
}
