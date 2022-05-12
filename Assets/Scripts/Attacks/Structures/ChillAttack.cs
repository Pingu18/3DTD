using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillAttack : MonoBehaviour
{
    public GameObject parentTower;
    private Stun stun;
    private TowerBuffHandler buffHandler;

    private Slow slow;

    public void setParentTower(GameObject parent)
    {
        parentTower = parent;
        stun = parentTower.GetComponent<Stun>();
        buffHandler = parentTower.GetComponent<TowerBuffHandler>();
        slow = parentTower.GetComponent<Slow>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && parentTower != null)
        {
            TowerObject towerObj = parentTower.GetComponent<TowerObject>();
            TowerBuffHandler buffHandler = parentTower.GetComponent<TowerBuffHandler>();

            other.gameObject.GetComponent<IDamageable>().queueDamage(towerObj.getDamage(), parentTower, false);
            slow.applySlow(other.gameObject);

            if (towerObj.getSpecialLevel() > 0)
            {
                other.gameObject.GetComponent<EnemyBuffHandler>().addChillStack();
                stun.checkForFreeze(other.gameObject);
            }
        }
    }
}
