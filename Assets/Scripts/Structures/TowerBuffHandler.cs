using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuffHandler : MonoBehaviour
{
    // Handles buffs and debuffs for towers

    // Lifesteal
    [Header("For Debugging")]
    [SerializeField] private bool lifestealEnabled;
    [SerializeField] private float lifestealPercent;

    private void Start()
    {
        lifestealEnabled = false;
        lifestealPercent = 0f;
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

    public float getLifestealPercent()
    {
        return lifestealPercent / 100;
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
        return newLifesteal > lifestealPercent;
    }

    public void setLifestealPercent(float percent)
    {
        lifestealPercent = percent;
    }
}
