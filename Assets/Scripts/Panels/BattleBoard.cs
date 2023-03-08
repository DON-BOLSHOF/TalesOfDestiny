using UnityEngine;

namespace Panels
{
    public class BattleBoard : AbstractPanelUtil
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _panels;

        public override void Show()
        {
            OnChangeState?.Invoke(true);
            _panels.SetActive(true); //Потом ЗАМЕНИТЬ НА ОТДЕЛЬНЫЕ ПАНЕЛИ - это лишь тест.
        }

        public override void Exit()
        {
            _panels.SetActive(true);
            OnChangeState?.Invoke(false);
        }
    }
}