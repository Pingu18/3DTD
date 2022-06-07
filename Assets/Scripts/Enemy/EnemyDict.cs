using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDict : MonoBehaviour
{
    // Reference to each different type of enemies
    [SerializeField] private GameObject testEnemy;
    [SerializeField] private GameObject testEnemyGiant;
    [SerializeField] private GameObject testEnemyHealer;

    [SerializeField] private GameObject waterBlob;
    [SerializeField] private GameObject grassBlob;
    [SerializeField] private GameObject earthBlobGiant;

    // Dictionary containing key-value pairs where: <name of enemy, prefab of enemy>
    Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating enemy dictionary... (EnemyDict)");

        // Add enemies into dictionary
        dict.Add(testEnemy.name, testEnemy);
        dict.Add(testEnemyGiant.name, testEnemyGiant);
        dict.Add(testEnemyHealer.name, testEnemyHealer);

        dict.Add(waterBlob.name, waterBlob);
        dict.Add(grassBlob.name, grassBlob);
        dict.Add(earthBlobGiant.name, earthBlobGiant);
    }

    public GameObject getEnemyPrefab(string name)
    {
        return dict[name];
    }
}
