using System.Collections;
using LevelManipulation;
using UnityEngine;
using Utils.Disposables;
using Vector2 = UnityEngine.Vector2;

[RequireComponent(typeof(Rigidbody2D))]
public class HeroBehaviour : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _velocity = 0.5f;
    [SerializeField] private Animator _animator;

    [SerializeField] private bool _inversed;
    
    private Rigidbody2D _rigidbody;
    private LevelBoard _board;

    private Coroutine _routine;

    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int Bow = Animator.StringToHash("Bow");

    private DisposeHolder _trash = new DisposeHolder();

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _board = FindObjectOfType<LevelBoard>();
        _trash.Retain(_board.GlobalHeroPosition.SubscribeAndInvoke(OnHeroPositionChanged));
    }
    
    private void OnHeroPositionChanged(Vector2 heroPosition)
    {
        var distance = Vector2.Distance(heroPosition, _rigidbody.position);
        
        UpgradeSpriteDirection(heroPosition - _rigidbody.position);
        StartRoutine(MoveToPosition(heroPosition, distance/_velocity)); 
    }

    private void StartRoutine(IEnumerator coroutine)
    {
        if(_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(coroutine);
    }

    private IEnumerator MoveToPosition(Vector2 position, float duration)
    {
        var delta = 0f;
        var currentPosition = _rigidbody.position;
        
        _animator.SetBool(IsRunning,true);
        while (_velocity * delta <= duration)
        {
            _rigidbody.position = Vector2.Lerp(currentPosition, position, _curve.Evaluate(_velocity * delta / duration));
            delta += Time.deltaTime;
            yield return null;
        }
        
        _animator.SetBool(IsRunning,false);
        _rigidbody.position = position;
        
        _animator.SetTrigger(Bow); // Не евойная ответственность, но из-за одной строчки перелопачивать корутины и менять Observable....
    }

    private void UpgradeSpriteDirection(Vector2 direction)
    {
        var multiplier = _inversed ? -1 : 1;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(multiplier, 1, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1f * multiplier, 1, 1);
        }
    }

    private void OnDestroy()
    {
        _trash.Dispose();
    }
}