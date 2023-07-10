using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceShip : MonoBehaviour
{
    [Header("Space Ship")]
    [SerializeField, Range(2f, 4.5f)] private float _radius = 4f; // Amplitude of sin and cos
    [SerializeField, Range(1f, 5f)] private float _movementSpeed = 4.5f; // Frequency of sin and cos 
    private float _posX, _posY = 0f;
    private float _currentTime; // Accumulated elapsed time (needed for sin, cos)
    private Vector3 _centerPoint = new(0, 0, 0); // Center of the circle
    private Vector2 _movementDirection;
    private Vector2 _rotationDirection;
    private Quaternion _lookRotation;
    private Rigidbody2D _rb;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnPoint;

    /// <summary>
    /// Calculate circular movement by manipulating the x-axis movement with cosinus and the y-axis movement with sinus.
    /// The clockwise/anticlockwise direction is handled by user input.
    /// </summary>
    private void CalculateMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            _currentTime += Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            _currentTime -= Time.deltaTime;
        }

        _posX = _centerPoint.x + (Mathf.Cos(_currentTime * _movementSpeed) * _radius);
        _posY = _centerPoint.y + (Mathf.Sin(_currentTime * _movementSpeed) * _radius);
        _movementDirection = new Vector2(_posX, _posY);
    }

    /// <summary>
    /// Calculate the rotation of the spaceship towards the center of the screen.
    /// </summary>
    private void CalculateRotation()
    {
        _rotationDirection = (_centerPoint - transform.position).normalized; 
        _lookRotation = Quaternion.LookRotation(Vector3.forward, _rotationDirection);
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
    /// Position the spaceship at the bottom of the screen.
    /// </summary>
    private void Start()
    {
        // Set the initial position of the spaceship (important in FixedUpdate()):
        _movementDirection = new Vector2(0, -_radius); 

        // Set the starting point on the sin/cos curve to match the initial position at the bottom of the screen --> prevent motion jumps at the beginning:
        _currentTime =  (1.5f / _movementSpeed) * Mathf.PI;

        _centerPoint = new Vector3(0, 0, 0);
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Handle user input.
    /// </summary>
    private void Update()
    {
        if (!Input.anyKey)
        {
            return;
        }

        CalculateMovement();
        CalculateRotation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// Perform the actual movement and rotation of the spaceship. 
    /// NOTE: Is called before Update(): so at the beginning, _movementDirection is not yet calcualted in Update() and therefore needs to be initialized in Start() as a specific start position is desired.
    /// </summary>
    private void FixedUpdate()
    {
        _rb.MovePosition(_movementDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.fixedDeltaTime * 700f);
    }
}