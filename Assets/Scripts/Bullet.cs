using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] private float _bulletSpeed = 6f;
    private Vector2 _targetPosition;

    private void Start()
    {
        _targetPosition = new Vector2(0,0);
    }

    /// <summary>
    /// Check if bullet reaches center point and destroy it.
    /// </summary>
    void Update()
    {
        if (transform.position.Equals(_targetPosition))
        {
            Destroy(gameObject, 0.0f);
        }
    }

    /// <summary>
    /// Perform the movement of the bullet towards the center point.
    /// </summary>
    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _bulletSpeed * Time.fixedDeltaTime); 
    }
}
