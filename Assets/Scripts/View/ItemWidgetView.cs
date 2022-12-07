using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ItemWidgetView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _itemIcon;

        public Image BackgroundIcon => _background;
        public Image ItemIcon => _itemIcon;
        
        public void SetViewData(CardView view)
        {
            _background.sprite = view.BackgroundView;
            _itemIcon.sprite = view.MainView;
        }
    }
}