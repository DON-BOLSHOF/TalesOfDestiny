using UnityEngine;

public class ParalaxEffect : MonoBehaviour
{
    [SerializeField] private float _lengthX;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _paralaxEffect;

    private float _startPosition;

    private void Start()
    {
        _startPosition = transform.position.x;
    }

    private void FixedUpdate()
    {
        var temp = (_target.transform.position.x * (1 - _paralaxEffect));
        var dist = _target.transform.position.x * _paralaxEffect;

        transform.position = new Vector3(_startPosition + dist, transform.position.y, transform.position.z);

        if (temp > _startPosition + _lengthX) _startPosition += _lengthX;
        else if (temp < _startPosition - _lengthX) _startPosition -= _lengthX;
    }
}
