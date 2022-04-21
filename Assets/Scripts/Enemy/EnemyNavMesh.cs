using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    private Transform destination;
    private NavMeshAgent navMeshAgent;

    private float chillTime = 0.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = destination.position;

        if (Time.time > chillTime)
        {
            navMeshAgent.speed = 3.5f;
        }
    }

    public void setDestination(Transform dest)
    {
        destination = dest;
    }

    public void setSpeed(float speed)
    {
        navMeshAgent.speed = speed;
        chillTime = Time.time + 1.5f;
    }
}
