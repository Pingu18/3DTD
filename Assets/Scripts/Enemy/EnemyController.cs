using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject commonEnemy;
    [SerializeField] private Transform destination;

    private GameObject newEnemy;
    private EnemyNavMesh enemyNavMesh;

    // Start is called before the first frame update
    void Start()
    {
        spawnTestEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        // Nothing atm...
    }

    private void spawnTestEnemies()
    {
        Debug.Log("Spawning test enemies...");
        for (int i = -10; i <= 10; i += 2)
        {
            spawnEnemy(commonEnemy, i, transform.localPosition.y - (float)0.6, 10);
        }
    }

    private void spawnEnemy(GameObject prefab, float x, float y, float z)
    {
        newEnemy = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        enemyNavMesh = newEnemy.GetComponent<EnemyNavMesh>();
        enemyNavMesh.setDestination(destination);
    }
}
