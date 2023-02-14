using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Widgets
{
    public class CardPropertyWidget : MonoBehaviour, IItemInstance<Sprite>
    {
        [SerializeField] private Image _image; 
        
        public void SetData(Sprite pack)
        {
            _image.sprite = pack;
        }
    }
}