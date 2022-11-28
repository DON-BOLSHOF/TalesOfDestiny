using System;
using UnityEngine;
using UnityEngine.UI;
using Widgets;

namespace Cards.SituationCards.PhysicalCard
{
    public class PhysicalCard : MonoBehaviour
    {
        [SerializeField] private CardViewWidget _cardViewWidget;

        public CardViewWidget CardViewWidget => _cardViewWidget;
    }

    [Serializable]
    public class CardViewWidget
    {
        [Header("Icons")]
        [SerializeField] private Image _backgroundIcon;
        [SerializeField] private Image _situationIcon;
        [SerializeField] private Transform _propertiesIconsPosition;
        
        [Space]
        [Header("Banners")]
        [SerializeField] private Text _nameBanner;
        [SerializeField] private Text _descriptionBanner;

        public Image BackgroundIcon => _backgroundIcon;
        public Image SituationIcon => _situationIcon;

        public Transform PropertiesPosition => _propertiesIconsPosition;

        public Text NameBanner => _nameBanner;
        public Text DescriptionBanner => _descriptionBanner;

        private PredefinedDataGroup<CardPropertyWidget, Sprite> _dataGroup; 
        
        public void SetWidgetData(PhysicalCardView cardView)
        {
            _dataGroup ??= new PredefinedDataGroup<CardPropertyWidget, Sprite>(PropertiesPosition);

            BackgroundIcon.sprite = cardView.BackgroundView;
            SituationIcon.sprite = cardView.MainView;
            _dataGroup.SetData(cardView.PropertyIcons);
            NameBanner.text = cardView.Id;
            DescriptionBanner.text = cardView.Wisecrack;
        }
    }
}
