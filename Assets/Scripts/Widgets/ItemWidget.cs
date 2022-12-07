using Cards;
using Controllers;
using UnityEngine;
using View;

namespace Widgets
{
    public sealed class ItemWidget : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ItemWidgetView _view;
        
        private LevelCard _card;
        private LevelController _controller;
        
        private static readonly int Swap = Animator.StringToHash("Swap");

        public void SetData(LevelCard card)
        {
            _card = card;
            _view.SetViewData(_card.View);
        }
        public void Click()
        {
            if (_card.Type == CardType.Situation)
            {
                _controller = FindObjectOfType<EventController>();
            }
            
            _animator.SetTrigger(Swap);
            _controller.Show(_card);
        }
    }
}
