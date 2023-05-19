using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] LeanTweenPath enemyPath;
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

        LeanTween.move(newEnemy, enemyPath.vec3, 20f).setOnComplete(() => 
        {
            newEnemy.GetComponent<Enemy>().Die();
        });

        LeanTween.delayedCall(spawnDelay, () => 
        {
            TrySpawnEnemie();
        });
    }
}
