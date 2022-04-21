using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TowerController : MonoBehaviour
{
    public float damage;
    public float range;
    public float fireRate;
    private float nextFire;
    GameObject currentTarget;
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    private SphereCollider detectionRadius;
    public GameObject attackFX;

    private void Start()
    {
        nextFire = 0.0f;
        detectionRadius = this.gameObject.transform.GetChild(0).GetComponent<SphereCollider>();
        detectionRadius.radius = range;
    }

    private void Update()
    {
        AttackCycle();
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
                print("Attacking: " + target.name);
                GameObject atk = Instantiate(attackFX, target.transform.position, Quaternion.identity);
                atk.transform.GetChild(0).GetComponent<VisualEffect>().Play();
                Destroy(atk, 1.5f);
                target.GetComponent<EnemyTest>().TakeDamage(damage, this.gameObject);
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
}
