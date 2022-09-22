using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 10f;

    [SerializeField, Range(0, 100f)] private float _maxAcceleration = 10.0f;

    [SerializeField] private Rect allowArea = new Rect(-5, -5, 10, 10);

    [SerializeField, Range(0f, 1f)] private float _bounciness = 0.5f;

    private Vector3 _velocity;


    private void Update()
    {
        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        ///期望达到的速度
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * _maxSpeed;
        ///加速度
        float maxSpeedChange = _maxAcceleration * Time.deltaTime;

        _velocity.x = Mathf.MoveTowards(_velocity.x, desiredVelocity.x, maxSpeedChange);
        _velocity.z = Mathf.MoveTowards(_velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = _velocity * Time.deltaTime;

        Vector3 newPosition = transform.localPosition + displacement;

        if (newPosition.x < allowArea.xMin)
        {
            newPosition.x = allowArea.xMin;
            _velocity.x = -_velocity.x * _bounciness;
        }
        else if (newPosition.x > allowArea.xMax)
        {
            newPosition.x = allowArea.xMax;
            _velocity.x = -_velocity.x * _bounciness;
        }
        else if (newPosition.z < allowArea.yMin)
        {
            newPosition.z = allowArea.yMin;
            _velocity.z = -_velocity.z * _bounciness;
        }
        else if (newPosition.z > allowArea.yMax)
        {
            newPosition.z = allowArea.xMax;
            _velocity.z = -_velocity.z * _bounciness;
        }


        transform.localPosition = newPosition;
    }
}