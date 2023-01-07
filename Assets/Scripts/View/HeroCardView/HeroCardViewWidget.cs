using UnityEngine;
using UnityEngine.UI;

namespace View.HeroCardView
{
    [RequireComponent(typeof(Animator))]
    public class HeroCardViewWidget : ItemWidgetView
    {
        [SerializeField] private Text _wiseCrackText;

        private Animator _animator;
        
        private static readonly int Swap = Animator.StringToHash("Swap");
        private Animator _viewAnimator;

        private void Start()
        {
            _viewAnimator = GetComponent<Animator>();
            _viewAnimator.SetTrigger(Swap);
        }

        public override void SetViewData(CardView cardView)
        {
            base.SetViewData(cardView);

            var heroCardView = (HeroCardView)cardView;

            _wiseCrackText.text = heroCardView.WiseCrack;
        }
    }
}