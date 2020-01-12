using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }
    [SerializeField] GameObject enemyPrefab;

    public int currentRound { get; private set; }
    public int currentMoney { get; private set; }
    public int currentScore { get; private set; }
    public int currentKills { get; private set; }
    public int currentMultiplier;
    public bool paused;
    public List<Transform> spawnPoints;
    public List<Transform> spawnPointsActive;
    public GameObject round_animator;

    void Awake()
    {
        GameObject master_obj = GameObject.FindGameObjectWithTag("Master");
        currentMultiplier = master_obj.GetComponent<MasterStats>().multiplier_level;
    }

    //Spawning
    void setUpSpawnList()
    {
        foreach (GameObject point in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(point.transform);
        }
    }
    public void add_kill(){
        currentKills++;
    }
    public void refeshSpawns()
    {
        spawnPointsActive.Clear();
        foreach (Transform point in spawnPoints)
        {
            if (point.GetComponent<SpawnList>().doorsToActive.Length == 0)
            {
                spawnPointsActive.Add(point);
            }
            else
            {
                foreach (GameObject doors in point.GetComponent<SpawnList>().doorsToActive)
                {
                    //if door open add point to active points
                    if (doors.GetComponent<door>().opened == true) { spawnPointsActive.Add(point); break; }
                }
            }
        }
    }
    private bool readyToSpawn;
    public int enemiesInRound;
    public int enemiesAlive;
    private int spawnedThisRound;
    private float timeBetweenSpawn;
    public float timeBetweenRounds = 20f;
    public bool loadingRound;
    // Use this for initialization
    void onPause()
    {
        if (paused == true) { Time.timeScale = 0;}
        if (paused == false) { Time.timeScale = 1; }
    }
    public void changeMoney(int value)
    {
        currentMoney += value;
    }
    public void changeScore(int value)
    {
        currentScore += value * currentMultiplier;
    }
    public IEnumerator nextRound()
    {
        loadingRound = true;
        readyToSpawn = false;
        round_animator.GetComponent<Animator>().SetTrigger("Change_round");
        changeScore(currentRound * 100);
        currentRound += 1;
        this.GetComponent<UIController>().refreshRoundText();
        enemiesInRound = currentRound*5;
        timeBetweenSpawn = 1;
        spawnedThisRound = 0;
        enemiesAlive = enemiesInRound;
        yield return new WaitForSeconds(timeBetweenRounds);
        readyToSpawn = true;
        loadingRound = false;
    }
    public IEnumerator playRound()
    {
        loadingRound = false;
        readyToSpawn = false;
        for (int i = 0; i < enemiesInRound; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawn);
            spawnEnemy(currentRound, currentRound, getSpawnPos());
            spawnedThisRound += 1;
        }
    }
    Transform getSpawnPos()
    {
        return spawnPointsActive[(int)Random.Range(0,spawnPointsActive.Count)];
    }
    public void TogglePause(){paused = !paused;}
    public void Pause(){ paused = true;}
    public void UnPause() { paused = false; }
    public void Startup()
    {
        setUpSpawnList();
        refeshSpawns();
        Debug.Log("Game manager starting...");
        status = ManagerStatus.Started;
        currentMoney = 0;
        currentMultiplier = 1;
        currentScore = 0;
        StartCoroutine(firstRound());
        GameObject master_obj;
        master_obj = GameObject.FindGameObjectWithTag("Master");
        currentMultiplier = master_obj.GetComponent<MasterStats>().multiplier_level;
    }
    public IEnumerator firstRound(){
        yield return new WaitForSeconds(10);
        StartCoroutine(nextRound());
    }
    // Update is called once per frame
    void Update()
    {
        //refeshSpawns();
        onPause();
        if (enemiesAlive <= 0  && currentRound > 0) { StartCoroutine(nextRound()); }
        if (readyToSpawn == true) { StartCoroutine(playRound()); }
    }
    void spawnEnemy(float health, int damage, Transform location)
    {
        float x_range = location.GetChild(0).localScale.x / 2f;
        float z_range = location.GetChild(0).localScale.z / 2f;
        Vector3 spawnPos = location.GetChild(0).position + new Vector3(Random.Range(-x_range, x_range),0,Random.Range(-z_range, z_range));
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, spawnPos, location.rotation);
        enemy.GetComponent<EnemyStats>().maxHealth = health;
        enemy.GetComponent<EnemyStats>().health = health;
        enemy.GetComponent<EnemyStats>().damage = damage;
    }
}