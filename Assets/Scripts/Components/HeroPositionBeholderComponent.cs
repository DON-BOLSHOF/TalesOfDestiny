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

        private void OnEnable()
        {
            if (_board == null)
                _board = FindObjectOfType<EventLevelBoard>();

            _trash.Retain(_board.HeroPosition.Subscribe(OnHeroPositionChanged));
        }

        private void OnDisable()
        {
            _trash.Dispose();
        }
    }
}