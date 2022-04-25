using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    private GameObject parentTower;
    private TowerObject towerCon;
    private Heal healScript;

    private void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        parentTower = this.transform.parent.gameObject;
        towerCon = parentTower.GetComponent<TowerObject>();
        healScript = parentTower.GetComponent<Heal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            towerCon.AddTarget(other.gameObject);
        }

        if (healScript)
        {
            if (other.gameObject.CompareTag("Structure"))
                healScript.AddStructure(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            towerCon.RemoveTarget(other.gameObject);
        }

        if (healScript)
        {
            if (other.gameObject.CompareTag("Structure"))
                healScript.RemoveStructure(other.gameObject);
        }
    }
}
