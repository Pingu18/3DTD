using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDict : MonoBehaviour
{
    // Reference to each different type of enemies
    [SerializeField] private GameObject testEnemy;

    // Dictionary containing key-value pairs where: <name of enemy, prefab of enemy>
    Dictionary<string, GameObject> dict = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Creating enemy dictionary... (EnemyDict)");

        // Add enemies into dictionary
        dict.Add(testEnemy.name, testEnemy);
    }

    public GameObject getEnemyPrefab(string name)
    {
        return dict[name];
    }
}
