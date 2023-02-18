using UnityEngine;
using Utils.Disposables;
using Utils.Interfaces;

namespace CodeAnimation
{
    public class BlightOnMouseCard : MonoBehaviour, ISubscriber
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _collider;

        private DisposeHolder _trash = new DisposeHolder();

        private static readonly int IlluminateKey = Animator.StringToHash("Illuminate");

        public void Subscribe()
        {
            var subscriber = GetComponentInParent<LevelBoardSubscriber>();
            _trash.Retain(subscriber.Subscribe(BlockRaycast));
        }

        private void OnMouseEnter()
        {
            _animator.SetBool(IlluminateKey, true);
        }

        private void OnMouseExit()
        {
            _animator.SetBool(IlluminateKey, false);
        }

        private void BlockRaycast(bool blocking)
        {
            _collider.enabled = !blocking; //При блокировании OnMouse... не должно происходить.
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}