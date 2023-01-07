using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class ItemWidgetView : MonoBehaviour // Вообще можно абстрактным сделать, но пусть на CardView лучше весит
    {
        [SerializeField] private Image _background;

        public Image BackgroundIcon => _background;
        
        public virtual void SetViewData(CardView view)
        {
            _background.sprite = view.BackgroundView;
        }
    }
}