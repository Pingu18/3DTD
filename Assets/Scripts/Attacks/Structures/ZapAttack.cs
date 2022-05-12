using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapAttack : MonoBehaviour
{
    public GameObject parentTower;
    private Bounce bounce;
    private Stun stun;
    public float radius;
    private int currentBounces;
    private bool isStarted;
    private bool isFinished;
    private Vector3 startPos;
    private GameObject target;
    private List<GameObject> nearbyTargets = new List<GameObject>();
    private float elapsedTime;
    private float percentageComplete;
    private float duration = 0.05f;
    private float damage;
    private bool stunned;

    // Special Upgrade
    public float stunDuration = 0.1f;

    private void Start()
    {
        stun = parentTower.GetComponent<Stun>();
        bounce = parentTower.GetComponent<Bounce>();
        stunned = false;
    }

    private void Update()
    {
        if (isStarted && !isFinished)
        {
            if (target != null)
            {
                elapsedTime += Time.deltaTime;
                percentageComplete = elapsedTime / duration;
                this.transform.position = Vector3.Lerp(startPos, target.transform.position, percentageComplete);

                if (this.transform.position == target.transform.position) // hit target
                {
                    currentBounces++;
                    nearbyTargets.Remove(target);
                    GameObject parent = null;
                    if (parentTower != null)
                    {
                        parent = parentTower;
                    }
                    target.GetComponent<IDamageable>().queueDamage(damage, parent, false);

                    if (parent != null)
                    {
                        TowerBuffHandler buffHandler = parent.GetComponent<TowerBuffHandler>();
                        TowerObject towerObj = parent.GetComponent<TowerObject>();

                        if (buffHandler.getLifestealEnabled())
                            towerObj.AddHP(towerObj.getDamage() * buffHandler.getLifestealPercent());
                    }

                    if (parent.GetComponent<TowerObject>().getSpecialLevel() > 0)
                    {
                        if (stun.checkForStun())
                        {
                            stun.stunTarget(target);
                            stunned = true;
                        }
                        //StartCoroutine(target.GetComponent<EnemyNavMesh>().applyStun(stunDuration));
                    }
                    if (GetNearbyTargets() > 0 && currentBounces <= bounce.getMaxBounces())
                    {
                        startPos = this.transform.position;
                        target = GetClosestTarget();
                        elapsedTime = 0f;
                    } else
                    {
                        Destroy(this.gameObject, 1.0f);
                        this.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                        isFinished = true;

                        if (stunned)
                        {
                            stun.resetAttacks();
                            stunned = false;
                        }
                    }
                }
            } else
            {
                Destroy(this.gameObject, 1.0f);
                this.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
                isFinished = true;
            }
        }
    }

    public void BeginAttack(GameObject currentPosition, GameObject target, int radius)
    {
        startPos = currentPosition.transform.position;
        this.target = target;
        this.radius = radius;
        this.currentBounces = 0;
        this.damage = parentTower.GetComponent<TowerObject>().getDamage();
        isStarted = true;
        isFinished = false;
    }

    private int GetNearbyTargets()
    {
        // once arrived, determine next target (if targets in radius)
        // if no targets in radius -> finish spell 
        //this.GetComponent<SphereCollider>().enabled = true;
        //print(nearbyTargets.Count + " nearby targets");
        return nearbyTargets.Count;
    }

    private GameObject GetClosestTarget()
    {
        float closestDist = 999;
        GameObject closestTarget = nearbyTargets[0];

        for (int i = 0; i < nearbyTargets.Count; i++)
        {
            if (nearbyTargets[i] != null)
            {
                float dist = Vector3.Distance(this.transform.position, nearbyTargets[i].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestTarget = nearbyTargets[i];
                }
            }
        }

        return closestTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            nearbyTargets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            nearbyTargets.Remove(other.gameObject);
        }
    }
}
