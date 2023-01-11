using Panels;
using UnityEngine;
using UnityEngine.UI;

namespace Components
{
    [RequireComponent(typeof(Image))]
    public class PanelUtilMaskImagesVisitor: MonoBehaviour
    {
        public Image Image { get; set; }

        private void OnEnable()
        {
            var util = GetComponentInParent(typeof(EventPanelUtil)) as EventPanelUtil;
            Image = GetComponent<Image>();
            
            Accept(util);
        }

        private void Accept(EventPanelUtil eventPanelUtil)
        {
            eventPanelUtil.DissolveAnimation.AddDynamicImage(Image); //Да нарушение DIP-a
        }
    }
}