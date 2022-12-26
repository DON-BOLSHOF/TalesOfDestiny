using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class HeroScript : MonoBehaviour
{
    [SerializeField] private float _velocity = 0.5f;
    
    private Rigidbody2D _rigidbody;

    private Vector2 _movement = new Vector2(1, 0);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movement * (_velocity * Time.deltaTime)); 
    }
}