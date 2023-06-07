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

    public ObjectPooler pooler;

    void Start()
    {
        TrySpawnEnemie();
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
            TrySpawnEnemie();
        });
    }
}
