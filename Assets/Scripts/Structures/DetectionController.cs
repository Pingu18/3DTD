using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    private GameObject parentTower;
    TowerController towerCon;
    private void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        parentTower = this.transform.parent.gameObject;
        towerCon = parentTower.GetComponent<TowerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && towerCon.canAttack)
        {
            towerCon.AddTarget(other.gameObject);
        }

        if (other.gameObject.CompareTag("Structure") && towerCon.canHeal)
        {
            towerCon.AddStructure(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && towerCon.canAttack)
        {
            towerCon.RemoveTarget(other.gameObject);
        }

        if (other.gameObject.CompareTag("Structure") && towerCon.canHeal)
        {
            towerCon.RemoveStructure(other.gameObject);
        }
    }
}
