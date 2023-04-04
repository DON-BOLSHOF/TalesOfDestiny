using UnityEngine;
using UnityEngine.UI;
using Utils.Interfaces;

namespace Components
{
    [RequireComponent(typeof(Image))]
    public class PanelUtilMaskImagesVisitor: MonoBehaviour
    {
        private Image Image { get; set; }

        private void OnEnable()
        {
            var util = GetComponentInParent(typeof(IDissolving)) as IDissolving;
            Image = GetComponent<Image>();
            
            Accept(util);
        }

        private void Accept(IDissolving iDissolving)
        {
            iDissolving.DissolveAnimation.AddDynamicImage(Image); //Да нарушение DIP-a
        }
    }
}