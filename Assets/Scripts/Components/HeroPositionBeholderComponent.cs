using LevelManipulation;
using UnityEngine;
using UnityEngine.Events;
using Utils.Disposables;

namespace Components
{
    public class HeroPositionBeholderComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent<bool> OnPositionChanged;
        
        private Vector2 _myPosition;
        private EventLevelBoard _board;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private void OnEnable()
        {
            if (_board == null)
                _board = GetComponentInParent<EventLevelBoard>();

            _trash.Retain(_board.LocalHeroMover.BoardHeroPosition.Subscribe(OnHeroPositionChanged));
        }

        public void SetPosition(Vector2 position)
        {
            _myPosition = position;
        }

        private void OnHeroPositionChanged(Vector2Int heroPosition)
        {
            var delta = _myPosition - heroPosition;
            var inside = Vector2.Dot(delta, delta) <= 1;
            
            OnPositionChanged?.Invoke(inside);
        }

        private void OnDisable()
        {
            _trash.Dispose();
        }
    }
}