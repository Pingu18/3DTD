using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffHandler : MonoBehaviour
{
    [SerializeField] private int chillStacks;

    private void Start()
    {
        chillStacks = 0;
    }

    public void addChillStack()
    {
        chillStacks += 1;
    }

    public void resetChillStacks()
    {
        chillStacks = 0;
    }

    public int getChillStacks()
    {
        return chillStacks;
    }
}
