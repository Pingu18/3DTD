using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    //[SerializeField] private Transform destination;
    private NavMeshAgent navMeshAgent;

    private float baseSpeed;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        updateBaseSpeed();
    }

    public void updateBaseSpeed()
    {
        baseSpeed = GetComponent<EnemyObject>().getMoveSpeed();
        navMeshAgent.speed = baseSpeed;
    }

    public void setDestination(Transform dest)
    {
        //destination = dest;

        Vector3 destination = new Vector3(dest.localPosition.x, dest.localPosition.y, dest.localPosition.z);
        navMeshAgent.SetDestination(destination);
    }

    public IEnumerator applySlow(float newSpeed, float duration)
    {
        // Get rid of the yield return
        // Add timer that ticks down from for example 5 -> 0 (update timer value in update function)
        // if the timer hits 0, then set speed back to base
        // this way, the timer can be refreshed on each hit

        navMeshAgent.speed = newSpeed;
        yield return new WaitForSeconds(duration);

        if (navMeshAgent)
            navMeshAgent.speed = baseSpeed;
    }

    public IEnumerator applyStun(float stunDuration)
    {
        navMeshAgent.speed = 0.0f;
        yield return new WaitForSeconds(stunDuration);

        if (navMeshAgent)
            navMeshAgent.speed = baseSpeed;
    }
}
