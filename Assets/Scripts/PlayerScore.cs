using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Scriptable object that contains the player score.
/// </summary>
[CreateAssetMenu(fileName = "PlayerScore.asset", menuName = "Gyruss Player Score", order = 1)]
public class PlayerScore : ScriptableObject
{
    /// <summary>
    /// Event that handles every change in score.
    /// </summary>
    public UnityEvent<int> OnUpdateScore;

    /// <summary>
    /// The player score that gets increased whenever enemies are hit.
    /// </summary>
    [SerializeField] private int _score;
    public int Score
    {
        get => _score; 
        set {
            _score += value;
            OnUpdateScore.Invoke(_score);
        }
    }

    /// <summary>
    /// Resets the score to zero.
    /// </summary>
    public void ResetScore()
    {
        _score = 0;
    }
}