using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    //the particular enemy you want to spawn here
    public GameObject enemyPrefab;
    public List<GameObject> spawnEnemies = new List<GameObject>();
    public float spawnRadiusScalar = 15.0f;
    public int maxSpawn = 5;
    public bool oneTimeSpawn = false;
    bool _shouldSpawn = false;
    public float spawnTimeInterval = 1.0f;
    float _pvtSpawnTimeInterval = 1.0f;
    public bool overrideAndClear = false;

    public SphereCollider spawnRadius;
    public AttackRangeBox combatZone;

    // Start is called before the first frame update
    void Start()
    {
        CreatePool();
        enemyPrefab.transform.position = gameObject.transform.position;
        if (oneTimeSpawn)
        {
            _pvtSpawnTimeInterval = 0.0f;
            spawnTimeInterval = 0.0f;
            for (int i = 0; i < spawnEnemies.Count; i++)
                SpawnEnemy();
        }
    }

    void CreatePool()
    {
        for (int i = 0; i < maxSpawn; i++)
        {
            spawnEnemies.Add(GameObject.Instantiate(enemyPrefab));
            spawnEnemies[i].gameObject.SetActive(false);
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = -1;

        if (_pvtSpawnTimeInterval > 0.0f)
            return;

        for (int i = 0; i < spawnEnemies.Count; i++)
        {
            if (!spawnEnemies[i].gameObject.activeSelf)
            {
                spawnIndex = i;
                break;
            }
        }
        if (spawnIndex == -1)
            return;//no availible enemy spawns

        float radius = spawnRadius.radius;

        var placeVec = new Vector3(Random.Range(-radius, radius), 0.0f, Random.Range(-radius, radius));
        spawnEnemies[spawnIndex].transform.position = gameObject.transform.position + placeVec * spawnRadiusScalar;
        spawnEnemies[spawnIndex].SetActive(true);
        spawnEnemies[spawnIndex].GetComponentInChildren<Enemy>().currentHP = enemyPrefab.GetComponentInChildren<Enemy>().enemyData.hp;
        _pvtSpawnTimeInterval = spawnTimeInterval;

    }

    public bool AnyEnemiesActive()
    {
        for (int i = 0; i < spawnEnemies.Count; i++)
        {
            if (spawnEnemies[i].activeSelf)
                return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (overrideAndClear)
            KillSpawnPoint();

        if (oneTimeSpawn)
            return;

        if(!AnyEnemiesActive())
            gameObject.SetActive(false);

        _pvtSpawnTimeInterval -= Time.deltaTime;
        if(_pvtSpawnTimeInterval <= 0.0f && combatZone.withinRange)
            SpawnEnemy();
    }

    void KillSpawnPoint()
    {
        foreach (var e in spawnEnemies)
            e.gameObject.SetActive(false);
    }

    public void ResetSpawnPoint()
    {
        foreach (var enemy in spawnEnemies)
        {
            enemy.SetActive(false);
            enemy.GetComponentInChildren<Enemy>().currentHP = enemyPrefab.GetComponentInChildren<Enemy>().enemyData.hp;
        }
        if (oneTimeSpawn)
        {
            _pvtSpawnTimeInterval = 0.0f;
            spawnTimeInterval = 0.0f;
            for (int i = 0; i < spawnEnemies.Count; i++)
                SpawnEnemy();
        }
    }
}
