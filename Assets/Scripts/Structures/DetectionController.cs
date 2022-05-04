using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionController : MonoBehaviour
{
    private GameObject parentObj;
    private TowerObject towerObj;
    private EnemyObject enemyObj;
    private Heal healScript;
    private Lifesteal lifestealScript;
    private DamageBuff damageBuff;
    private DamageDebuff damageDebuff;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        parentObj = transform.parent.gameObject;
        towerObj = parentObj.GetComponent<TowerObject>();
        enemyObj = parentObj.GetComponent<EnemyObject>();
        healScript = parentObj.GetComponent<Heal>();
        lifestealScript = parentObj.GetComponent<Lifesteal>();
        damageDebuff = parentObj.GetComponent<DamageDebuff>();
        damageBuff = parentObj.GetComponent<DamageBuff>();
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
            if (transform.parent.gameObject.tag == "Structure")
            {
                // If tower has Heal, add targets to be healed
                if (other.gameObject.CompareTag("Structure"))
                    healScript.addTarget(other.gameObject);
            } else if (transform.parent.gameObject.tag == "Enemy")
            {
                // If enemy has Heal, add targets to be healed
                if (other.gameObject.CompareTag("Enemy"))
                    healScript.addTarget(other.gameObject);
            }
        }

        if (lifestealScript && lifestealScript.getLifestealBuff())
        {
            if (transform.parent.gameObject.tag == "Structure")
            {
                // If tower has Lifesteal, add targets to gain effect
                if (other.gameObject.CompareTag("Structure"))
                    lifestealScript.addTarget(other.gameObject);
            }
            else if (transform.parent.gameObject.tag == "Enemy")
            {
                // If enemy has Lifesteal script, add targets to gain effect
                if (other.gameObject.CompareTag("Enemy"))
                {
                    lifestealScript.addTarget(other.gameObject);
                }
            }
        }

        if (damageBuff)
        {
            if (other.gameObject.CompareTag("Structure"))
                damageBuff.AddStructure(other.gameObject);
        }

        if (damageDebuff)
        {
            if (other.gameObject.CompareTag("Enemy"))
                damageDebuff.AddTarget(other.gameObject);
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
            if (transform.parent.gameObject.tag == "Structure")
            {
                // If tower has Heal, remove targets to be healed
                if (other.gameObject.CompareTag("Structure"))
                    healScript.removeTarget(other.gameObject);
            }
            else if (transform.parent.gameObject.tag == "Enemy")
            {
                // If enemy has Heal, remove targets to be healed
                if (other.gameObject.CompareTag("Enemy"))
                    healScript.removeTarget(other.gameObject);
            }
        }

        if (lifestealScript && lifestealScript.getLifestealBuff())
        {
            if (transform.parent.gameObject.tag == "Structure")
            {
                // If tower has Lifesteal, remove targets to gain effect
                if (other.gameObject.CompareTag("Structure"))
                    lifestealScript.removeTarget(other.gameObject);
            }
            else if (transform.parent.gameObject.tag == "Enemy")
            {
                // If enemy has Lifesteal script, remove targets to gain effect
                if (other.gameObject.CompareTag("Enemy"))
                {
                    //lifestealScript.removeTarget(other.gameObject);
                }
            }
        }

        if (damageBuff)
        {
            if (other.gameObject.CompareTag("Structure"))
                damageBuff.RemoveStructure(other.gameObject);
        }

        if (damageDebuff)
        {
            if (other.gameObject.CompareTag("Enemy"))
                damageDebuff.RemoveTarget(other.gameObject);
        }
    }
}
