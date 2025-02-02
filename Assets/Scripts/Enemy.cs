using UnityEngine;

/// <summary>
/// The enemy that the player has to shoot at.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Enemy Movement")]
    [SerializeField, Range(2f, 3.7f)] private float _maxRadius = 3.7f; // Amplitude of sin and cos
    [SerializeField, Range(0.1f, 5f)] private float _movementSpeed = 2f; // Frequency of sin and cos 
    private float _currentRadius = 0.05f;
    private float _posX, _posY;
    private float _direction; 
    private Vector2 _movementDirection;
    private Vector2 _centerPoint;
    private Rigidbody2D _rb;
    private Quaternion _lookRotation;

    [Header("Scoring")]
    [SerializeField] private PlayerScore _playerScore;
    [SerializeField] private int _scorePointsPro = 10;
    [SerializeField] private int _scorePointsMedium = 5;
    [SerializeField] private int _scorePointsEasy = 1;
    [SerializeField] private float _smallScoreRadius = 1.5f;
    [SerializeField] private float _bigScoreRadius = 2.5f;
    private int _scorePoints;

    /// <summary>
    /// Calculate circular movement (only anticlockwise) by manipulating the x-axis movement with cosine and the y-axis movement with sine.
    /// </summary>
    private void CalculateMovement()
    {
        _direction += Time.deltaTime; 
        _posX = _centerPoint.x + Mathf.Cos(_direction * _movementSpeed) * _currentRadius; 
        _posY = _centerPoint.y + Mathf.Sin(_direction * _movementSpeed) * _currentRadius;
        _movementDirection = new Vector2(_posX, _posY);
    }

    /// <summary>
    /// Calculate rotation towards the movement direction.
    /// </summary>
    private void CalculateRotation()
    {
        float angle = Vector2.SignedAngle(Vector2.right, _movementDirection);
        Vector3 targetRotation = new Vector3(0, 0, angle);
        _lookRotation = Quaternion.Euler(targetRotation);
    }

    /// <summary>
    /// Destroy bullet and enemy when enemy collides with bullet.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Bullet>() is Bullet)
        {
            _playerScore.Score = _scorePoints;
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }

    /// <summary>
    /// Initializes values at the start of the game.
    /// </summary>
    private void Start()
    {
        _movementDirection = new Vector2(_currentRadius, _currentRadius); // Starting position of sin(0)/cos(0)
        transform.position = _movementDirection; // Spawning right on the circle instead of the center to prevent motion jumps
        _ = transform.eulerAngles.y + 180; // Rotation towards movement direction 
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Slowly increase the circle diameter, decrease the score value for higher diameter.
    /// </summary>
    private void Update()
    {
        CalculateMovement();
        CalculateRotation();

        // Scoring
        if (_currentRadius < _maxRadius)
        {
            _currentRadius += 0.005f;
            
            if (_currentRadius <= _smallScoreRadius)
            {
                _scorePoints = _scorePointsPro;
            }
            if (_currentRadius > _smallScoreRadius && _currentRadius <= _bigScoreRadius)
            {
                _scorePoints = _scorePointsMedium;
            } else if (_currentRadius > _bigScoreRadius)
            {
                _scorePoints = _scorePointsEasy;
            }
        }
    }

    /// <summary>
    /// Performs the actual movement of the enemy. 
    /// </summary>
    private void FixedUpdate()
    {
        float angularMovement = 500f;
        _rb.MovePosition(_movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, angularMovement);
    }
}