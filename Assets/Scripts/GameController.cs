using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the game. Responsible for the enemy spawning and the representation of the score in the UI.
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("Enemy Spawning")]
    [SerializeField, Range(0, 10)] private int _waveCount = 10;
    [SerializeField, Range(2f, 20f)] private float _waveInterval;
    [SerializeField, Range(0, 10)] private int _enemyCount = 5;
    [SerializeField] private Enemy _enemyPrefab;
    private float _spawnTimer = 1f;

    [Header("Scoring")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private PlayerScore _playerScore;

    /// <summary>
    /// Spawns an enemy at the center of the screen.
    /// </summary>
    private void SpawnEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab, new Vector2(0,0), Quaternion.identity);
        enemy.gameObject.SetActive(true);
    }

    /// <summary>
    /// Continuously spawns enemies in small time-intervals.
    /// </summary>
    private IEnumerator SpawnEnemyWave()
    {
        for (int i = 0; i < _enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }
    }
    
    /// <summary>
    /// Increases the player's score.
    /// </summary>
    /// <param name="scoreNumber"></param>
    private void UpdateScore(int scoreNumber)
    {
        _scoreText.text = "Score: " + scoreNumber; 
    }

    private void Start()
    {
        _playerScore.ResetScore();
        _playerScore.OnUpdateScore.AddListener(UpdateScore);
    }

    /// <summary>
    /// Manages the spawning of the enemy waves.
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