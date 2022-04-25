using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform destination;
    private NavMeshAgent navMeshAgent;

    private float speed;

    [SerializeField] private bool isSlowed = false;
    [SerializeField] private bool isStunned = false;

    private float slowTime = 0.0f;
    private float stunTime = 0.0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        speed = GetComponent<EnemyObject>().getMoveSpeed();

    }

    void Update()
    {
        navMeshAgent.destination = destination.position;

        if (Time.time > slowTime)
        {
            isSlowed = false;
        }

        if (Time.time > stunTime)
        {
            isStunned = false;
        }

        if (!isSlowed && !isStunned)
        {
            navMeshAgent.speed = speed;
        } else if (isStunned)
        {
            navMeshAgent.speed = 0.0f;
        }
    }

    public void setDestination(Transform dest)
    {
        Debug.Log("Setting destination to: " + dest.localPosition);
        destination = dest;
    }

    public void applySlow(float slowPercent)
    {
        float newSpeed = speed * slowPercent;
        if (newSpeed < navMeshAgent.speed)
        {
            navMeshAgent.speed = newSpeed;
        }
        slowTime = Time.time + 1.5f;
        isSlowed = true;
    }

    public void applyStun(float stunDuration)
    {
        stunTime = Time.time + stunDuration;
        isStunned = true;
    }
}
