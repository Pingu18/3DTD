using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffHandler : MonoBehaviour
{
    [SerializeField] private int chillStacks;
    [SerializeField] private float freezeDuration;

    private void Start()
    {
        freezeDuration = 1.0f;
        chillStacks = 0;
    }

    public void addChillStack()
    {
        chillStacks += 1;

        if (chillStacks >= 3)
        {
            StartCoroutine(GetComponent<EnemyNavMesh>().applyStun(freezeDuration));
            chillStacks = 0;
        }
    }

    public int getChillStacks()
    {
        return chillStacks;
    }
}
