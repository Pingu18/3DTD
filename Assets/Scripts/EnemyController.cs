using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject commonEnemy;

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
            GameObject newEnemy = Instantiate(commonEnemy, new Vector3(i, transform.localPosition.y - (float)0.6, 10), Quaternion.identity, transform);
        }
    }
}
