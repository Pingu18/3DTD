using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // JSON file containing information about each waves
    [SerializeField] private TextAsset enemyWavesJSON;

    // Reference to each different type of enemies
    [SerializeField] private GameObject commonEnemy;

    // Where the enemies spawn
    [SerializeField] private Transform spawnPoint;

    // Destination that the enemies are trying to reach
    [SerializeField] private Transform destination;

    private GameObject newEnemy;
    private EnemyNavMesh enemyNavMesh;

    private int currWave;
    private int currGroup;
    private int enemiesAlive;
    private bool waveCleared;

    private void Start()
    {
        //Debug.Log(commonEnemy.name.ToString());
        //StartCoroutine(test());
        StartCoroutine(startWaves(enemyWavesJSON));
        //spawnTestEnemies();
    }

    private void Update()
    {
        // Nothing atm...
    }

    /*
    private IEnumerator test()
    {
        Debug.Log("Running test...");

        yield return StartCoroutine(waitUntilTest());
        Debug.Log("Waited 10 seconds...");

        yield return 0;
    }

    private IEnumerator waitUntilTest()
    {
        Debug.Log("Running waitUntilTest");
        yield return new WaitForSeconds(10);
    }
    */

    private IEnumerator startWaves(TextAsset jsonFile)
    {
        // Reads individual wave information from JSON file
        Waves allWaves = JsonUtility.FromJson<Waves>(jsonFile.text);

        foreach (Wave wave in allWaves.waves)
        {
            // Variable List:
            // wave.wave -> Wave Number

            currWave = wave.wave;

            foreach (Group groups in wave.groups)
            {
                // Variable List:
                // groups.groupNum -> Group Number

                currGroup = groups.groupNum;

                // Add groupsToSpawn to JSON -> N groups to spawn at the same time
                // Add amountOfGroups to JSON -> how many groups there are
                // for loop index by N
                // add variable: currentGroupToSpawn = 1
                // whenever coroutine starts, increment currentGroupToSpawn
                // when group dies, spawn next group (currentGroupToSpawn)
                // until currentGroupToSpawn > amountOfGroups

                // Put this logic into coroutine
                foreach (Enemy enemy in groups.enemies)
                {
                    // Variable List:
                    // enemy.enemyName -> Name of enemy prefab
                    // enemy.toSpawn -> how much to spawn

                    Debug.Log("Starting Wave " + currWave + ": Group " + currGroup);
                    enemiesAlive = enemiesAlive + enemy.toSpawn;

                    // Compare enemy names to decide which enemy to spawn
                    if (enemy.enemyName.Equals(commonEnemy.name))
                    {
                        for (int i = 1; i <= enemy.toSpawn; i++)
                        {
                            spawnEnemy(commonEnemy);
                            yield return new WaitForSeconds(0.5f);
                        }
                    }

                    // Set waveCleared to false
                    waveCleared = false;

                    // Wait until wave is cleared before spawning next wave
                    yield return StartCoroutine(waitUntilClear());
                }
            }
        }

        Debug.Log("All Waves Cleared!");
        yield return 0;
    }

    private IEnumerator waitUntilClear()
    {
        Debug.Log("Waiting until wave clear...");
        yield return new WaitUntil(() => waveCleared == true);
    }

    public void decrementEnemiesAlive()
    {
        enemiesAlive -= 1;

        if (enemiesAlive == 0)
        {
            Debug.Log("Wave " + currWave + ": Group " + currGroup + " Cleared...");
            waveCleared = true;
        }
    }

    private void spawnTestEnemies()
    {
        Debug.Log("Spawning test enemies...");
        for (int i = -10; i <= 10; i += 2)
        {
            spawnEnemy(commonEnemy);
        }
    }

    private void spawnEnemy(GameObject prefab)
    {
        newEnemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity, transform);
        enemyNavMesh = newEnemy.GetComponent<EnemyNavMesh>();
        enemyNavMesh.setDestination(destination);
    }
}