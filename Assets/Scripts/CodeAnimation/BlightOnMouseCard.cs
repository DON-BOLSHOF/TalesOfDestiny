using UnityEngine;

namespace CodeAnimation
{
    public class BlightOnMouseCard : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int IlluminateKey = Animator.StringToHash("Illuminate");
        protected void OnMouseEnter()
        {
            _animator.SetBool(IlluminateKey, true);
        }

        protected void OnMouseExit()
        {
            _animator.SetBool(IlluminateKey, false);
        }
    }
}
