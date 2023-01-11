using UnityEngine;

namespace Panels
{
    public class EnemyPanelUtil : AbstractPanelUtil
    {
        [SerializeField] private Animator _animator;
        
        private static readonly int ExitKey = Animator.StringToHash("Exit");

        public override void Show()
        {
            OnChangeState(true);
            _typingAnimation.HideText();
            
            StartRoutine(_typingAnimation.TypeText(), ref _typingRoutine);
        }
        
        public override void Exit()
        {
            OnSkipText();
            _typingAnimation.HideText();
            
            _animator.SetTrigger(ExitKey);
        }

        public void OnExited()//В аниматоре вызовится
        {
            OnChangeState?.Invoke(false);
            gameObject.SetActive(false);
        }
    }
}