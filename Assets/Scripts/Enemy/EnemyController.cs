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
    private int groupsSpawned = 0;
    private bool groupsCleared;

    private void Start()
    {
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
            // wave.groupsToSpawn -> how many groups to spawn at the same time (how many paths are there)
            // wave.groups -> array of groups

            currWave = wave.wave;
            //Debug.Log("Groups To Spawn: " + wave.groupsToSpawn);

            foreach (Group groups in wave.groups)
            {
                // Variable List:
                // groups.groupNum -> Group Number
                // groups.spawnX -> spawn X position
                // groups.spawnY -> spawn Y position
                // groups.spawnZ -> spawn Z position
                // groups.enemies -> array of enemies

                currGroup = groups.groupNum;
                groupsCleared = false;

                // Start spawning the enemies
                StartCoroutine(spawnEnemies(groups));
                groupsSpawned++;

                // Wait until the same number of groups spawned is equal to waves.groupsToSpawn before checking for clear condition
                yield return StartCoroutine(checkGroupsSpawned(wave.groupsToSpawn));
            }
        }
        

        Debug.Log("All Waves Cleared!");
        yield return 0;
    }

    private IEnumerator checkGroupsSpawned(int groupsToSpawn)
    {
        if (groupsSpawned != groupsToSpawn)
            yield return 0;
        else
            yield return StartCoroutine(waitUntilClear());
    }

    private IEnumerator spawnEnemies(Group groups)
    {
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
                    // Set spawnPoint of the enemy before spawning
                    setSpawnPoint(groups.spawnX, groups.spawnY, groups.spawnZ);
                    spawnEnemy(commonEnemy);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        yield return 0;
    }

    private IEnumerator waitUntilClear()
    {
        Debug.Log("Waiting until wave clear...");
        yield return new WaitUntil(() => groupsCleared == true);
        groupsSpawned = 0;
    }

    public void decrementEnemiesAlive()
    {
        enemiesAlive -= 1;

        if (enemiesAlive == 0)
        {
            Debug.Log("Wave " + currWave + ": Group " + currGroup + " Cleared...");
            groupsCleared = true;
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

    private void setSpawnPoint(float x, float y, float z)
    {
        spawnPoint.position = new Vector3(x, y, z);
    }
}