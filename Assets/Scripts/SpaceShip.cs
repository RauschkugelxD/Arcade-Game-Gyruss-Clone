using UnityEngine;

/// <summary>
/// The spaceship that is controlled by the player.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : MonoBehaviour
{
    [Header("Space Ship Movement")]
    [SerializeField, Range(2f, 4.5f)] private float _radius = 4f; // Amplitude of sin and cos
    [SerializeField, Range(1f, 5f)] private float _movementSpeed = 4.5f; // Frequency of sin and cos 
    private float _posX, _posY;
    private int _movementDirection = 1;
    private float _movement;
    private Vector3 _spawnPoint;
    private Vector2 _targetPosition;
    private Vector2 _rotationDirection;
    private Quaternion _lookRotation;
    private Rigidbody2D _rb;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    /// <summary>
    /// Calculates circular movement by manipulating the x-axis movement with cosine and the y-axis movement with sine.
    /// The clockwise/anticlockwise direction is handled by user input.
    /// </summary>
    private void CalculateMovementAndRotation()
    {
        bool leftArrow = Input.GetKey(KeyCode.LeftArrow);
        bool rightArrow = Input.GetKey(KeyCode.RightArrow);
        bool keyA = Input.GetKey(KeyCode.A);
        bool keyD = Input.GetKey(KeyCode.D);

        // Determine direction when keys are getting pressed
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            _movementDirection = -1;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            _movementDirection = 1;
        }

        // Perform movement when movement keys are pressed
        if (leftArrow || rightArrow || keyA || keyD)
        {
            // Change direction when a key is lifted but another directional key is still pressed
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                if (rightArrow || keyD)
                {
                    _movementDirection = 1;
                }
            } else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                if (leftArrow || keyA)
                {
                    _movementDirection = -1;
                }
            }

            // Increase or decrease the x-value of sin/cos based on the current direction
            if (_movementDirection == -1)
            {
                _movement -= Time.deltaTime;
            } else if (_movementDirection == 1)
            {
                _movement += Time.deltaTime;
            }

            // Calculate sind/cos values for circular movement
            _posX = _spawnPoint.x + Mathf.Cos(_movement * _movementSpeed) * _radius;
            _posY = _spawnPoint.y + Mathf.Sin(_movement * _movementSpeed) * _radius;
            _targetPosition = new Vector2(_posX, _posY);

            // Calculate rotation of the spaceship towards the center of the screen
            _rotationDirection = (_spawnPoint - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(Vector3.forward, _rotationDirection); 
        }
    }

    /// <summary>
    /// Fires a bullet in front of the spaceship.
    /// </summary>
    private void ShootBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        bullet.gameObject.SetActive(true);
    }

    /// <summary>
    /// Positions the spaceship at the bottom of the screen.
    /// </summary>
    private void Start()
    {
        // Set the initial position of the spaceship (important in FixedUpdate())
        _targetPosition = new Vector2(0, -_radius);

        // Set the starting point on the sin/cos curve to match the initial position at the bottom of the screen to prevent motion jumps at the beginning
        _movement = 1.5f / _movementSpeed * Mathf.PI;

        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handles user input.
    /// </summary>
    private void Update()
    {
        if (!Input.anyKey)
        {
            return;
        }

        CalculateMovementAndRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// Performs the actual movement and rotation of the spaceship.
    /// </summary>
    /// <remarks>Is called before Update(): At the beginning, _movementDirection is not yet calculated in Update() and therefore needs to be initialized in Start() as a specific start position is desired.</remarks>
    private void FixedUpdate()
    {
        float angularSpeed = 400f;
        _rb.MovePosition(_targetPosition);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.fixedDeltaTime * angularSpeed);
    }
}