using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDict : MonoBehaviour
{
    [Header("Attacks")]
    [SerializeField] private GameObject chillAttackFX;
    [SerializeField] private GameObject blastAttackFX;
    [SerializeField] private GameObject zapAttackFX;

    Dictionary<string, GameObject> attackDict = new Dictionary<string, GameObject>();

    private void Start()
    {
        attackDict.Add("Water Tower", chillAttackFX);
        attackDict.Add("Fire Tower", blastAttackFX);
        attackDict.Add("Lightning Tower", zapAttackFX);
    }

    public string getAttackName(string towerName)
    {
        switch (towerName)
        {
            case "Fire Tower":
                return "Blast";
            case "Water Tower":
                return "Chill";
            case "Lightning Tower":
                return "Zap";
            default:
                return null;
        }
    }

    public GameObject getAttackFX(string towerName)
    {
        return attackDict[towerName];
    }
}
