using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerScore.asset", menuName = "Gyruss Player Score", order = 1)]
public class PlayerScore : ScriptableObject
{
    public UnityEvent<int> OnUpdateScore;

    [SerializeField] private int _score;
    public int Score
    {
        get => _score; 
        set {
            _score += value;
            OnUpdateScore.Invoke(_score);
        }
    }

    public void ResetScore()
    {
        _score = 0;
    }
}
