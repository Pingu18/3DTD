using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TowerController : MonoBehaviour
{
    BuildController buildCon;

    [Header("Tower Details")]
    public bool canAttack;
    public float damage;
    public float range;
    public float fireRate;
    public float maxHP;
    public bool canHeal = false;
    public float healAmount;
    public float healRate;
    private float currentHP;
    private float nextFire;
    private float nextHeal;
    private float clearNulls;
    GameObject currentTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>(); // list of enemies in radius
    [SerializeField] private List<GameObject> structures = new List<GameObject>(); // list of structures in radius
    private GameObject detectionRadius;
    private Vector3 detectionScale;
    private bool drawRadius = false;
    public GameObject attackFX;

    private void Start()
    {
        buildCon = FindObjectOfType<BuildController>();
        nextFire = 0.0f;
        nextHeal = 0.0f;
        clearNulls = 0.0f;
        detectionRadius = this.gameObject.transform.GetChild(0).gameObject;
        detectionScale = new Vector3(range, range / 3, range);
        detectionRadius.transform.localScale = detectionScale;
        currentHP = maxHP;
    }

    private void Update()
    {
        if (canAttack)
        {
            AttackCycle();
        }

        if (canHeal)
        {
            HealCycle();
        }

        if (buildCon.getInBuild() && drawRadius)
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            detectionRadius.GetComponent<MeshRenderer>().enabled = false;
        }

    }

    private void AttackCycle()
    {
        if (targets.Count > 0) // enemies in range
        {
            currentTarget = SelectBestTarget();
            AttackTarget(currentTarget);
        }

        if (Time.time > clearNulls)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                {
                    targets.Remove(targets[i]);
                }
            }
            clearNulls = Time.time + 10.0f;
        }
    }

    private void HealCycle()
    {
        if (Time.time > nextHeal)
        {
            for (int i = 0; i < structures.Count; i++)
            {
                if (structures[i] != null && structures[i].GetComponent<TowerController>().GetCurrentHP() < structures[i].GetComponent<TowerController>().maxHP)
                {
                    GameObject heal = Instantiate(attackFX, structures[i].transform.position, Quaternion.identity);
                    heal.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    Destroy(heal, 1.0f);
                    structures[i].GetComponent<TowerController>().AddHP(healAmount);
                }
            }
            nextHeal = Time.time + (1 / healRate);
        }

        if (Time.time > clearNulls)
        {
            for (int i = 0; i < structures.Count; i++)
            {
                if (structures[i] == null)
                {
                    structures.Remove(structures[i]);
                }
            }
            clearNulls = Time.time + 10.0f;
        }
    }


    public void AddTarget(GameObject target)
    {
        targets.Add(target);
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
    }

    private void AttackTarget(GameObject target)
    {
        if (Time.time > nextFire)
        {
            if (currentTarget != null)
            {
                //print("Attacking: " + target.name);
                if (attackFX.name == "Blast")
                {
                    GameObject atk = Instantiate(attackFX, target.transform.position, Quaternion.identity);
                    atk.GetComponent<BlastAttack>().target = target;
                    atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    target.GetComponent<IDamageable>().queueDamage(damage, this.gameObject);
                    //target.GetComponent<EnemyTest>().TakeDamage(damage, this.gameObject);
                    Destroy(atk, 1.0f);
                } else if (attackFX.name == "Chill")
                {
                    GameObject atk = Instantiate(attackFX, target.transform.position, Quaternion.identity);
                    atk.GetComponent<ChillAttack>().parentTower = this.gameObject;
                    atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    Destroy(atk, 1.2f);
                } else if (attackFX.name == "Heal")
                {

                }

                nextFire = Time.time + (1 / fireRate);
            }
        }
    }

    private GameObject SelectBestTarget()
    {
        float closestDist = 999;
        GameObject closestTarget = targets[0];

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                if (Vector3.Distance(this.transform.position, targets[i].transform.position) < closestDist)
                {
                    closestDist = Vector3.Distance(this.transform.position, targets[i].transform.position);
                    closestTarget = targets[i].gameObject;
                }
            }
        }
        return closestTarget;
    }

    public void AddStructure(GameObject structure)
    {
        structures.Add(structure);
    }

    public void RemoveStructure(GameObject structure)
    {
        targets.Remove(structure);
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            this.gameObject.GetComponent<Outline>().enabled = true;
            //drawRadius = true;
        } else
        {
            this.gameObject.GetComponent<Outline>().enabled = false;
            //drawRadius = false;
        }
    }

    public float GetCurrentHP()
    {
        return this.currentHP;
    }

    public void AddHP(float amount)
    {
        if (this.currentHP + amount > maxHP)
        {
            this.currentHP = maxHP;
        } else
        {
            this.currentHP += amount;
        }
    }

}
