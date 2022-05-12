using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAttack : MonoBehaviour
{
    private TowerBuffHandler buffHandler;
    public GameObject parentTower;
    public GameObject initialTarget;
    private float splashDamage;

    public void setParams(GameObject parent, GameObject target, float damage)
    {
        parentTower = parent;
        initialTarget = target;
        splashDamage = damage * 0.5f;

        buffHandler = parentTower.GetComponent<TowerBuffHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && parentTower != null && other.gameObject != initialTarget)
        {
            other.gameObject.GetComponent<IDamageable>().queueDamage(splashDamage, parentTower, false);
        }
    }
}
