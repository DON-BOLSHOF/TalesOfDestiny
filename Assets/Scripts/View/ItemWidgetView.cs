using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ItemWidgetView : MonoBehaviour // Вообще можно абстрактным сделать, но пусть на CardView лучше весит
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _itemIcon;

        public Image BackgroundIcon => _background;
        public Image ItemIcon => _itemIcon;
        
        public virtual void SetViewData(CardView view)
        {
            _background.sprite = view.BackgroundView;
            _itemIcon.sprite = view.MainView;
        }
    }
}