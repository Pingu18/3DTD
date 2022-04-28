using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillAttack : MonoBehaviour
{
    public GameObject parentTower;

    private Slow slow;

    public void setParentTower(GameObject parent)
    {
        parentTower = parent;
        slow = parentTower.GetComponent<Slow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && parentTower != null)
        {
            other.gameObject.GetComponent<IDamageable>().queueDamage(parentTower.GetComponent<TowerObject>().getDamage(), parentTower);
            slow.applySlow(other.gameObject);
        }
    }
}
