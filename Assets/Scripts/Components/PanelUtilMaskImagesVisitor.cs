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
            var util = GetComponentInParent(typeof(PanelUtil)) as PanelUtil;
            Image = GetComponent<Image>();
            
            Accept(util);
        }

        private void Accept(PanelUtil panelUtil)
        {
            panelUtil.DissolveAnimation.AddDynamicImage(Image); //Да нарушение DIP-a
        }
    }
}