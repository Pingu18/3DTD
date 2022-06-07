using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuffHandler : MonoBehaviour
{
    [Header("Display for Debugging")]
    [SerializeField] private int chillStacks;
    [SerializeField] private bool hasLightMark;
    [SerializeField] private bool hasDarkMark;

    private void Start()
    {
        chillStacks = 0;
    }

    public void addChillStack()
    {
        chillStacks += 1;
        updateDisplay();
    }

    public void resetChillStacks()
    {
        chillStacks = 0;
    }

    public void applyLightMark(TowerObject towerObj)
    {
        if (!hasLightMark)
        {
            hasLightMark = true;
            updateDisplay();
        }

        checkMarks(towerObj);
    }

    public void applyDarkMark(TowerObject towerObj)
    {
        if (!hasDarkMark)
        {
            hasDarkMark = true;
            updateDisplay();
        }

        checkMarks(towerObj);
    }

    public void checkMarks(TowerObject towerObj)
    {
        if (hasLightMark && hasDarkMark)
        {
            float dmg = towerObj.getDamage() + towerObj.getSpecial();

            hasLightMark = false;
            hasDarkMark = false;

            updateDisplay();

            GetComponent<EnemyObject>().queueDamage(dmg, towerObj.gameObject, false);
        }
    }

    public void updateDisplay()
    {
        if (hasLightMark)
        {
            // display light mark
        } else
        {
            // do not display light mark
        }

        if (hasDarkMark)
        {
            // display dark mark
        } else
        {
            // do not display dark mark
        }

        if (chillStacks > 0)
        {
            // display chill stacks
        } else
        {
            // do not display chill stacks
        }
    }

    public int getChillStacks()
    {
        return chillStacks;
    }

    public bool getLightMark()
    {
        return hasLightMark;
    }

    public bool getDarkMark()
    {
        return hasDarkMark;
    }
}
