using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    // Movement:
    [SerializeField] private float _currentRadius;
    [SerializeField, Range(2f, 3.7f)] private float _maxRadius = 3.7f; // Amplitude of sin and cos
    [SerializeField, Range(1f, 5f)] private float _movementSpeed = 2f; // Frequency of sin and cos 
    private float _posX, _posY = 0f;
    private float _currentTime = 0f; // Accumulated elapsed time (needed for sin, cos)
    private Vector2 _movementDirection;
    private Vector2 _centerPoint = new Vector2(0, 0); // Center of the circle
    private Rigidbody2D _rb;
    Quaternion _lookRotation;

    // Scoring:
    [SerializeField] private PlayerScore _playerScore;
    private int _scoreNumber;

    /// <summary>
    /// Calculate circular movement (only anticlockwise) by manipulating the x-axis movement with cosinus and the y-axis movement with sinus.
    /// </summary>
    private void CalculateMovement()
    {
        _currentTime += Time.deltaTime; 
        _posX = _centerPoint.x + Mathf.Cos(_currentTime * _movementSpeed) * _currentRadius; 
        _posY = _centerPoint.y + Mathf.Sin(_currentTime * _movementSpeed) * _currentRadius;
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
        if (collider.GetComponent<Bullet>() is Bullet bullet)
        {
            _playerScore.Score = _scoreNumber;
            Destroy(gameObject);
            Destroy(collider.gameObject);
        }
    }

    private void Start()
    {
        _currentRadius = 0.5f;
        _movementDirection = new Vector2(_currentRadius, _currentRadius); // Starting position of sin(0)/cos(0)
        transform.position = _movementDirection; // Spawning right on the circle instead of the center to prevent motion jumps
         _ = transform.eulerAngles.y + 180; // Rotation towards movement direction 
        _rb = GetComponent<Rigidbody2D>();

        _scoreNumber = 10;
    }

    /// <summary>
    /// Slowly increase the circle diameter, decrease the score value for higher diameter.
    /// </summary>
    private void Update()
    {
        CalculateMovement();
        CalculateRotation();

        // Scoring:
        if (_currentRadius < _maxRadius)
        {
            _currentRadius += 0.005f;

            if (_currentRadius >= 1.5f && _currentRadius <= 2.5f)
            {
                _scoreNumber = 5;
            } else if (_currentRadius >= 2.5f)
            {
                _scoreNumber = 1;
            }
        }
    }

    /// <summary>
    /// Performs the actual movement of the enemy. 
    /// </summary>
    private void FixedUpdate()
    {
        _rb.MovePosition(_movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, 20f);
    }
}
