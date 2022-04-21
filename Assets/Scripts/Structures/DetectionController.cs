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
        if (other.gameObject.CompareTag("Enemy"))
        {
            towerCon.AddTarget(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            towerCon.RemoveTarget(other.gameObject);
        }
    }
}
