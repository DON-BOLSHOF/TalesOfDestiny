using System;
using UnityEngine;
using UnityEngine.UI;

namespace View.HeroCardView
{
    [RequireComponent(typeof(Animator))]
    public class HeroCardViewWidget : CardViewWidget
    {
        [SerializeField] private Image _background;

        [SerializeField] private Text _wiseCrackText;

        private static readonly int Swap = Animator.StringToHash("Swap");
        private Animator _viewAnimator;
        
        private void Awake()
        {
            _viewAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _viewAnimator.SetTrigger(Swap);
        }

        public override void SetViewData(CardView cardView)
        {
            _background.sprite = cardView.BackgroundView;

            var heroCardView = (HeroCardView)cardView;

            _wiseCrackText.text = heroCardView.WiseCrack;
        }
    }
}