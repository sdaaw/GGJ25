using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{
    public List<EnemyWave> enemyWaves = new List<EnemyWave>();
    public EnemyWave currentWave;
    // public int nextWaveIndex = 0;

    [SerializeField]
    private TMP_Text _enemyRemainingText;

    public void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // TODO: needs last wave logic
            SpawnWave(enemyWaves[nextWaveIndex]);
            currentWave = enemyWaves[nextWaveIndex];
            nextWaveIndex++;
        }*/

        if (_enemyRemainingText != null && currentWave != null)
        {
            _enemyRemainingText.text = $"Enemies: {currentWave.enemiesRemaining}";
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
        wave.currentWaveEnemies.Add(e);
    }

    public void DespawnWave()
    {
        if (currentWave != null && currentWave.currentWaveEnemies.Count > 0)
        {
            for (int i = currentWave.currentWaveEnemies.Count - 1; i == 0; i--)
            {
                Destroy(currentWave.currentWaveEnemies[i].gameObject);
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
    public int enemiesRemaining
    {
        get { return currentWaveEnemies.Count; }
    }
}