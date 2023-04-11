using System.Collections;
using CodeAnimation;
using Components;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class MagicScroll : MonoBehaviour //Вроде наговнокодил и работает, в след раз сделай все через твины!
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private MouseEnterComponent _mouseEnterComponent;
        [SerializeField] private DissolveAnimation _dissolveAnimation;

        private static readonly int ShowVelocityKey = Animator.StringToHash("ShowVelocity");
        private static readonly int ExitVelocityKey = Animator.StringToHash("ExitVelocity");
        private static readonly int ShowKey = Animator.StringToHash("Show");

        private Coroutine _routine;
        private bool _iStarted = false;

        private void Start()
        {
            _mouseEnterComponent.OnEnter += Reshow;
            _mouseEnterComponent.OnExit += Exit;
        }

        public void Show()
        {
            _iStarted = true;
            StartRoutine(Showing());
        }

        private void Reshow()
        {
            if (_iStarted)
                StartRoutine(Reshowing());
        }

        private IEnumerator Showing()
        {
            yield return _dissolveAnimation.StartAnimation();

            ChangeAnimatorStates(true);
        }

        private void ChangeAnimatorStates(bool value)
        {
            _animator.SetFloat(ShowVelocityKey, value ? 1 : -1);
            _animator.SetFloat(ExitVelocityKey, value ? -1 : 1);
            _animator.SetBool(ShowKey, value);
        }

        private IEnumerator Reshowing()
        {
            ChangeAnimatorStates(true);

            yield return _dissolveAnimation.StartAnimation();
        }

        private void Exit()
        {
            StartRoutine(Exiting());
        }

        private IEnumerator Exiting()
        {
            ChangeAnimatorStates(false);
            yield return
                new WaitForSeconds(_animator.GetCurrentAnimatorClipInfo(0)
                    .Length); //Потом поработай пожалуйста лучше с Dotween
            yield return _dissolveAnimation.EndAnimation();


            _iStarted = false;
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_routine != null)
                StopCoroutine(_routine);

            _routine = StartCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            _dissolveAnimation.OnEmerged -= Reshow;
            _dissolveAnimation.OnStartDissolving -= Exit;
        }
    }
}