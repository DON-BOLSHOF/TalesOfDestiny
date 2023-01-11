using UnityEngine;

public class RepetativeMovingObject : MonoBehaviour
{
    [SerializeField] private float _boarderLength;
    [SerializeField] private float _speed;

    private Vector2 _startPosition;
    float distance;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        distance += Time.deltaTime * _speed * 0.01f;
        transform.position = Vector2.Lerp(_startPosition,
            new Vector2(_startPosition.x + _boarderLength, transform.position.y), distance);
        if (distance >= 1) distance = 0;
    }
}