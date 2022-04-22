using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TowerController : MonoBehaviour
{
    BuildController buildCon;
    public float damage;
    public float range;
    public float fireRate;
    private float nextFire;
    GameObject currentTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    private GameObject detectionRadius;
    private Vector3 detectionScale;
    private bool drawRadius = false;
    public GameObject attackFX;

    private void Start()
    {
        buildCon = FindObjectOfType<BuildController>();
        nextFire = 0.0f;
        detectionRadius = this.gameObject.transform.GetChild(0).gameObject;
        detectionScale = new Vector3(range, range / 3, range);
        detectionRadius.transform.localScale = detectionScale;
    }

    private void Update()
    {
        AttackCycle();

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
                    target.GetComponent<IDamageable>().takeDamage(damage, this.gameObject);
                    //target.GetComponent<EnemyTest>().TakeDamage(damage, this.gameObject);
                    Destroy(atk, 1.5f);
                } else if (attackFX.name == "Chill")
                {
                    GameObject atk = Instantiate(attackFX, target.transform.position, Quaternion.identity);
                    atk.GetComponent<ChillAttack>().parentTower = this.gameObject;
                    atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                    Destroy(atk, 1.5f);
                }

                nextFire = Time.time + (1 / fireRate);
            }
        }
    }

    public void RemoveTargetOnDeath(GameObject target)
    {
        targets.Remove(target);
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
}
