using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> allEnemies = new List<GameObject>();
    [SerializeField] LeanTweenPath enemyPath;
    [SerializeField] float travelTime;
    [SerializeField] float spawnDelay;
    [SerializeField] int slimeAmount;

    [SerializeField] GameObject startButton, stopButton;

    public ObjectPooler pooler;
    bool canSpawn;

    private void Start()
    {
        StopSpawning();
    }
    public void SpawnButton()
    {
        if (canSpawn)
        {
            StopSpawning();
        }
        else
        {
            StartSpawning();
        }
    }
    public void StartSpawning()
    {
        startButton.SetActive(false);
        stopButton.SetActive(true);
        TrySpawnEnemie();
        canSpawn = true;
    }

    public void StopSpawning()
    {
        startButton.SetActive(true);
        stopButton.SetActive(false);
        canSpawn = false;
    }
    void TrySpawnEnemie()
    {
        int randomSlime = Random.Range(0, slimeAmount);
        GameObject newEnemy = pooler.SpawnFromPool($"{randomSlime}", transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f), transform.rotation);
        allEnemies.Add(newEnemy);

        LeanTween.move(newEnemy, enemyPath.vec3, travelTime).setOnComplete(() =>
        {
            newEnemy.GetComponent<Enemy>().Die(false);
        });

        LeanTween.delayedCall(spawnDelay, () =>
        {
            if (canSpawn)
            {
                TrySpawnEnemie();
            }
        });
    }
}
