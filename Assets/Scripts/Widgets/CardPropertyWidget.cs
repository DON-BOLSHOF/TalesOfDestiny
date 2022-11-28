using UnityEngine;
using UnityEngine.UI;

namespace Widgets
{
    public class CardPropertyWidget : MonoBehaviour, IItemInstance<Sprite>
    {
        [SerializeField] private Image _image; 
        
        public void SetData(Sprite data)
        {
            _image.sprite = data;
        }
    }
}