using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] LeanTweenPath enemyPath;
    [SerializeField] float spawnDelay;
    [SerializeField] enum EnemyType { GreenSlime, BlueSlime, PurpleSlime }
    [SerializeField] EnemyType enemyType;

    public ObjectPooler pooler;

    void Start()
    {
        TrySpawnEnemie();
    }

    void TrySpawnEnemie()
    {
        GameObject newEnemy = pooler.SpawnFromPool("GreenSlime", transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f), transform.rotation);

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
