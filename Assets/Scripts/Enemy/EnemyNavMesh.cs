using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    //[SerializeField] private Transform destination;
    private NavMeshAgent navMeshAgent;

    private float baseSpeed;
    [SerializeField] private bool stunned;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        stunned = false;
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

    public bool compareSpeed(float newSpeed)
    {
        if (newSpeed < navMeshAgent.speed)
            return true;

        return false;
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

        if (!stunned)
        {
            navMeshAgent.speed = newSpeed;
        }
        yield return new WaitForSeconds(duration);

        if (navMeshAgent && !stunned)
            navMeshAgent.speed = baseSpeed;
    }

    public IEnumerator applyStun(float stunDuration)
    {
        stunned = true;
        Vector3 destination = navMeshAgent.destination;
        navMeshAgent.SetDestination(gameObject.transform.position);
        yield return new WaitForSeconds(stunDuration);

        if (navMeshAgent)
        {
            navMeshAgent.SetDestination(destination);
            stunned = false;
        }
    }
}
