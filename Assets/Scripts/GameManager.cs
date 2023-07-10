using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField, Range(0, 10)] private int _waveCount = 10;
    [SerializeField, Range(2f, 20f)] private float _waveInterval;
    [SerializeField, Range(0, 10)] private int _enemyCount = 5;
    [SerializeField] private Enemy _enemyPrefab;
    private float _spawnTimer;

    // Scoring:
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private PlayerScore _playerScore;

    /// <summary>
    /// Spawn an enemy at the center of the screen.
    /// </summary>
    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab, new Vector2(0,0), Quaternion.identity);
        enemy.gameObject.SetActive(true);
    }

    /// <summary>
    /// Continuosly spawn enemies in small time intervals.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemyWave()
    {
        for (int i = 0; i < _enemyCount; i++)  // while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    /// <summary>
    /// Increase the player's score.
    /// </summary>
    /// <param name="scoreNumber"></param>
    private void UpdateScore(int scoreNumber)
    {
        _scoreText.text = "Score: " + scoreNumber; 
    }

    private void Start()
    {
        _spawnTimer = 2f; // Initial waiting time for the first wave
        _playerScore.ResetScore();
        _playerScore.OnUpdateScore.AddListener(UpdateScore);
    }

    /// <summary>
    /// Manage spawning of the enemy waves.
    /// </summary>
    private void Update()
    {
        if (_waveCount > 0)
        {
            if (_spawnTimer <= 0)
            {
                StartCoroutine(SpawnEnemyWave());
                _waveCount--;
                _spawnTimer = _waveInterval; // Reset
            }
            else
            {
                _spawnTimer -= Time.deltaTime;
            }
        }
    }
}