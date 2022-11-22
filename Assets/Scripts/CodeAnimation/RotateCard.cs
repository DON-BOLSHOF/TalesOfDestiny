using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeAnimation
{
    [RequireComponent(typeof(Animator))]
    public class RotateCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Animator _animator;

        private static readonly int SwapKey = Animator.StringToHash("Spin");
    
        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _animator.SetBool(SwapKey, true);
        }
 
        public void OnPointerExit(PointerEventData eventData)
        {
            _animator.SetBool(SwapKey, false);
        }
    }
}
