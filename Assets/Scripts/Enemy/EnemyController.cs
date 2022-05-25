using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyController : MonoBehaviour
{
    // JSON file containing information about each waves
    [SerializeField] private TextAsset enemyWavesJSON;

    [SerializeField] private TMP_Text notStartedText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text skipText;

    // Where the enemies spawn
    [SerializeField] private Transform spawnPoint;

    // Destination that the enemies are trying to reach
    [SerializeField] private Transform destination;

    [Header("References")]
    [SerializeField] private GameObject towerContainer;
    [SerializeField] private GameObject level;
    private LivesController livesCon;

    private PathController pathController;

    // EnemyDict that contains a reference to each different type of enemy
    private EnemyDict enemyDict;

    private GameObject newEnemy;
    private EnemyNavMesh enemyNavMesh;

    private float timeToWait = 30f;

    private bool levelStarted;
    private int currWave;
    private int currGroup;
    private int enemiesAlive;
    private int groupsSpawned = 0;
    private bool groupsCleared;

    private float timerValue;
    private bool timerOn = false;
    private bool checkSkipOn = false;
    private bool skip;

    private void Start()
    {
        Debug.Log("Getting enemyDict... (EnemyController)");
        enemyDict = GetComponent<EnemyDict>();

        pathController = level.GetComponent<PathController>();

        Debug.Log("Initializing parameters... (EnemyController)");
        timerValue = timeToWait;
        levelStarted = false;

        StartCoroutine(startGame());
        livesCon = FindObjectOfType<LivesController>();
        livesCon.UpdateEnemiesLeft(enemiesAlive);
    }

    private void Update()
    {
        if (!levelStarted)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                levelStarted = true;
                notStartedText.enabled = false;
            }
        }

        if (timerOn)
        {
            if (timerValue > 0)
            {
                timerValue -= Time.deltaTime;
            }
            else
            {
                timerValue = 0;
                finishTimer();
            }

            timerText.text = "Until next wave: " + Mathf.FloorToInt(timerValue % 60).ToString();
        }

        if (checkSkipOn)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Skip!");
                skip = true;
                endTimer();
            }
        }
    }

    private IEnumerator startGame()
    {
        yield return new WaitUntil(() => levelStarted == true);
        StartCoroutine(startWaves(enemyWavesJSON));
    }

    private IEnumerator startWaves(TextAsset jsonFile)
    {
        // Reads individual wave information from JSON file
        Waves allWaves = JsonUtility.FromJson<Waves>(jsonFile.text);
        
        foreach (Wave wave in allWaves.waves)
        {
            // Variable List: wave.wave/groupsToSpawn/groups
            currWave = wave.wave;
            skip = false;

            foreach (Group groups in wave.groups)
            {
                // Variable List: groups.groupNum/spawnX/spawnY/spawnZ/enemies
                currGroup = groups.groupNum;
                groupsCleared = false;

                // Set total enemies alive in group (for enemy clear checking)
                enemiesAlive += groups.totalEnemies;
                livesCon.UpdateEnemiesLeft(enemiesAlive);

                if (!pathController.checkPathAdded(groups.path) && groups.path != 0)
                    pathController.addPath(groups.path);

                // Start spawning the enemies
                StartCoroutine(spawnEnemies(groups));
                groupsSpawned++;

                // Wait until the same number of groups spawned is equal to waves.groupsToSpawn before checking for clear condition
                yield return StartCoroutine(checkGroupsSpawned(wave.groupsToSpawn));
            }

            // When wave ends, heal all towers to full
            healAllTowers();

            // Add yield return for set amount of time or check for player skip
            Debug.Log("Break between waves...");
            yield return StartCoroutine(downtime());
        }

        Debug.Log("All Waves Cleared!");
        yield return 0;
    }

    private void startTimer()
    {
        timerOn = true;
        timerText.enabled = true;
        skipText.enabled = true;
    }

    public void endTimer()
    {
        timerOn = false;
        timerText.enabled = false;
        skipText.enabled = false;
    }

    private void resetTimer()
    {
        timerValue = timeToWait;
    }

    private void finishTimer()
    {
        if (timerValue <= 0)
        {
            skip = true;
            endTimer();
        }
    }

    private void startCheckSkip()
    {
        checkSkipOn = true;
    }

    private void resetCheckSkip()
    {
        checkSkipOn = false;
    }

    private void healAllTowers()
    {
        Transform tfm = towerContainer.transform;
        Debug.Log("Healing all towers...");

        for (int i = 0; i < tfm.childCount; i++)
        {
            TowerObject towerObj = tfm.GetChild(i).gameObject.GetComponentInChildren<TowerObject>();
            towerObj.healToFull();
        }
    }

    private IEnumerator downtime()
    {
        startCheckSkip();
        startTimer();

        yield return new WaitUntil(() => skip == true);

        resetTimer();
        resetCheckSkip();
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
            // Variable List: enemy.enemyName / toSpawn / toWait
            Debug.Log("Starting Wave " + currWave + ": Group " + currGroup);

            for (int i = 1; i <= enemy.toSpawn; i++)
            {
                // Set spawnPoint of the enemy before spawning
                setSpawnPoint(groups.spawnX, groups.spawnY, groups.spawnZ);
                spawnEnemy(enemyDict.getEnemyPrefab(enemy.enemyName));
            }
            yield return new WaitForSeconds(enemy.toWait);
        }

        yield return 0;
    }

    private IEnumerator waitUntilClear()
    {
        Debug.Log("Waiting until clear...");
        yield return new WaitUntil(() => groupsCleared == true);
        groupsSpawned = 0;
        pathController.clearPaths();
    }

    public void decrementEnemiesAlive()
    {
        enemiesAlive -= 1;
        livesCon.UpdateEnemiesLeft(enemiesAlive);

        if (enemiesAlive == 0)
        {
            Debug.Log("Wave " + currWave + ": Group " + currGroup + " Cleared...");
            groupsCleared = true;
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

    // Function used for development only (to test enemy spawning)
    private void spawnTestEnemies()
    {
        Debug.Log("Spawning test enemies...");
        for (int i = -10; i <= 10; i += 2)
        {
            spawnEnemy(enemyDict.getEnemyPrefab("TestEnemy"));
        }
    }
}