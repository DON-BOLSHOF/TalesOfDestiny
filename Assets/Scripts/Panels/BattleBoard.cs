using UnityEngine;

namespace Panels
{
    public class BattleBoard : AbstractPanelUtil
    {
        [SerializeField] private Animator _animator;
        
        public override void Show()
        {
            OnChangeState?.Invoke(true);
        }

        public override void Exit()
        {
            OnChangeState?.Invoke(false);
        }
    }
}