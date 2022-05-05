using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private int maxBounces;

    [SerializeField] private int bounceLevel;

    private void Start()
    {
        maxBounces = 2;
    }

    public void setMaxBounces(int newMax)
    {
        maxBounces = newMax;
    }

    public void setBounceLevel(int newLevel)
    {
        bounceLevel = newLevel;
    }

    public int getMaxBounces()
    {
        return maxBounces;
    }

    public int getBounceLevel()
    {
        return bounceLevel;
    }
}
