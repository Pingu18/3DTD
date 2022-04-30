using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyController : MonoBehaviour
{
    [SerializeField] TMP_Text displayMoney;

    private GameObject structure;

    private int currentMoney;

    private void Start()
    {
        setMoney(100000);
        updateDisplay();
    }

    public void addMoney(int toAdd)
    {
        currentMoney += toAdd;
        updateDisplay();
    }

    public void removeMoney(int toRemove)
    {
        currentMoney -= toRemove;
        updateDisplay();
    }

    public void setMoney(int toSet)
    {
        currentMoney = toSet;
        updateDisplay();
    }

    public void setStructure(GameObject obj)
    {
        structure = obj;
    }

    private void updateDisplay()
    {
        displayMoney.text = "$ " + currentMoney.ToString();
    }

    public bool checkSufficientMoney(int cost)
    {
        if (currentMoney >= cost)
        {
            removeMoney(cost);
            return true;
        } else
            return false;
    }

    public int getMoney()
    {
        return currentMoney;
    }
}
