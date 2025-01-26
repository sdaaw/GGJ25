using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public List<EnemyWave> enemyWaves = new List<EnemyWave>();
    public int nextWaveIndex = 0;
    public float townAgroRange = 100;

    [SerializeField]
    private TMP_Text _enemyRemainingText;

    private BubbleCharacterController _player;
    private void Start()
    {
        _player = FindFirstObjectByType<BubbleCharacterController>();
    }

    public void Update()
    {
        if (_player != null)
        {
            CheckPlayerRangeToTown();
        }
        else
        {
            _player = FindFirstObjectByType<BubbleCharacterController>();
        }
    }

    public void CheckPlayerRangeToTown()
    {
        foreach (EnemyWave wave in enemyWaves)
        {
            if (Vector3.Distance(wave.waveSpawnOwner.transform.position, _player.transform.position) <= townAgroRange)
            {
                if (wave.enemiesRemaining <= 0)
                {
                    SpawnWave(wave);
                }  
            }
        }
    }



    public void SpawnWave(EnemyWave ew)
    {
        for (int i = 0; i < ew.enemyAmount; i++)
        {
            SpawnEnemy(ew.waveEnemies[Random.Range(0, ew.waveEnemies.Count)], ew);
        }
    }

    public void SpawnEnemy(Enemy enemy, EnemyWave wave)
    {
        var e = Instantiate(enemy, wave.waveSpawnPositions[Random.Range(0, wave.waveSpawnPositions.Count)].position, Quaternion.identity);
        // e.CurrentHealth = e.CurrentHealth + (100 / GameManager.instance.MoneyReward);
        e.SetTarget(FindFirstObjectByType<BubbleCharacterController>().transform);
        e.waveOwner = wave; 
        wave.currentWaveEnemies.Add(e);
    }

    public void DespawnWave(EnemyWave wave)
    {
        if (wave != null && wave.currentWaveEnemies.Count > 0)
        {
            for (int i = wave.currentWaveEnemies.Count - 1; i == 0; i--)
            {
                Destroy(wave.currentWaveEnemies[i].gameObject);
            }
        }
    }
}

[System.Serializable]
public class EnemyWave
{
    public int enemyAmount;
    public List<Enemy> waveEnemies = new List<Enemy>();
    public List<Transform> waveSpawnPositions = new List<Transform>();
    public List<Enemy> currentWaveEnemies = new List<Enemy>();

    public GameObject waveSpawnOwner;
    public int enemiesRemaining
    {
        get { return currentWaveEnemies.Count; }
    }
}