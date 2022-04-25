using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    private GameObject parentObj;
    private TowerObject towerObj;
    private EnemyObject enemyObj;
    private Heal healScript;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        parentObj = transform.parent.gameObject;
        towerObj = parentObj.GetComponent<TowerObject>();
        enemyObj = parentObj.GetComponent<EnemyObject>();
        healScript = parentObj.GetComponent<Heal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (towerObj)
        {
            if (other.gameObject.CompareTag("Enemy"))
                towerObj.AddTarget(other.gameObject);
        }

        if (enemyObj)
        {
            if (other.gameObject.CompareTag("Structure"))
                enemyObj.addTarget(other.gameObject);
        }

        if (healScript)
        {
            if (other.gameObject.CompareTag("Structure"))
                healScript.AddStructure(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (towerObj)
        {
            if (other.gameObject.CompareTag("Enemy"))
                towerObj.RemoveTarget(other.gameObject);
        }

        if (enemyObj)
        {
            if (other.gameObject.CompareTag("Structure"))
                enemyObj.removeTarget(other.gameObject);
        }

        if (healScript)
        {
            if (other.gameObject.CompareTag("Structure"))
                healScript.RemoveStructure(other.gameObject);
        }
    }
}
