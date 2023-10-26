using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] List<Wave> waves;
    public float timer { get; private set; } = 0f;
    float lastSpawn = 0f;
    Wave CurrentWave
    {
        get
        {
            int index = (int)(timer / 60);
            index = Mathf.Min(index, waves.Count - 1);
            return waves[index];
        }
    }

    private void Awake()
    {
        EnemyController.spawnedEnemies = new();
        EnemyController.despawnedEnemies = new();
        for(int i = 0; i < 300; i++)
        {
            var enemy = Instantiate(enemyPrefab).GetComponent<EnemyController>();
            enemy.transform.parent = transform;
            enemy.Despawn();
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        var wave = CurrentWave;
        while (EnemyController.spawnedEnemies.Count < wave.minimunEnemyCount)
        {
            foreach(var enemy in wave.enemies)
                if (EnemyController.despawnedEnemies.Count > 0)
                    EnemyController.despawnedEnemies[0].Spawn(enemy);
        }
        if (timer > lastSpawn + 1f)
        {
            lastSpawn = timer;
            foreach (var enemy in wave.enemies)
                if (EnemyController.despawnedEnemies.Count > 0)
                    EnemyController.despawnedEnemies[0].Spawn(enemy);
        }
    }

    [System.Serializable]
    class Wave
    {
        public List<EnemyType> enemies;
        public int minimunEnemyCount = 10;
    }
}
