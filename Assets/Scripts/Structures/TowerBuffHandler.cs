using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuffHandler : MonoBehaviour
{
    // Handles buffs and debuffs for towers

    // Lifesteal
    [Header("For Debugging")]
    [SerializeField] private bool lifestealEnabled;
    [SerializeField] private float lifestealStrength;

    private void Start()
    {
        lifestealEnabled = false;
        lifestealStrength = 0f;
    }

    public void enableSpecial(string towerName)
    {
        switch (towerName)
        {
            case "Grass Tower":
                GetComponent<Lifesteal>().enableUpgrade();
                break;
        }
    }

    public float getLifestealStrength()
    {
        return lifestealStrength;
    }

    public bool getLifestealEnabled()
    {
        return lifestealEnabled;
    }
    public void enableLifesteal()
    {
        lifestealEnabled = true;
    }

    public void disableLifesteal()
    {
        lifestealEnabled = false;
    }

    public bool compareLifesteal(float newLifesteal)
    {
        return newLifesteal > lifestealStrength;
    }

    public void setLifestealStrength(float strength)
    {
        lifestealStrength = strength;
    }
}
