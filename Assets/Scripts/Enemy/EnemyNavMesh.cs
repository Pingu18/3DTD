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
        //navMeshAgent.areaMask = 0;
    }

    private void Update()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            OffMeshLinkData data = navMeshAgent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * navMeshAgent.baseOffset;

            navMeshAgent.transform.position = Vector3.MoveTowards(navMeshAgent.transform.position, endPos, navMeshAgent.speed * Time.deltaTime);

            if (navMeshAgent.transform.position == endPos)
                navMeshAgent.CompleteOffMeshLink();
        }
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
