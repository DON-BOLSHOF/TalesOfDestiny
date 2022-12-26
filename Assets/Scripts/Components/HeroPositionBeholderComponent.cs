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
        private LevelManager _manager;

        private readonly DisposeHolder _trash = new DisposeHolder();

        public void SetPosition(Vector2 position)
        {
            _myPosition = position;
        }

        private void OnHeroPositionChanged(Vector2 heroPosition)
        {
            var delta = _myPosition - heroPosition;
            var inside = Vector2.Dot(delta, delta) <= 1;
            
            OnPositionChanged?.Invoke(inside);
        }

        private void OnEnable()
        {
            if (_manager == null)
                _manager = FindObjectOfType<LevelManager>();

            _trash.Retain(_manager.HeroPosition.Subscribe(OnHeroPositionChanged));
        }

        private void OnDisable()
        {
            _trash.Dispose();
        }
    }
}