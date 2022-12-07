using UnityEngine;
using UnityEngine.UI;
using Widgets;

namespace View
{
    public sealed class EventCardViewWidget : ItemWidgetView
    {
        [Header("Icons")]
        [SerializeField] private Transform _propertiesIconsPosition;
        
        [Space]
        [Header("Banners")]
        [SerializeField] private Text _nameBanner;
        [SerializeField] private Text _descriptionBanner;
        
        public Transform PropertiesPosition => _propertiesIconsPosition;

        public Text NameBanner => _nameBanner;
        public Text DescriptionBanner => _descriptionBanner;

        private PredefinedDataGroup<CardPropertyWidget, Sprite> _dataGroup; 
        
        public void SetViewData(EventCardView cardView)
        {
            base.SetViewData(cardView);
            _dataGroup ??= new PredefinedDataGroup<CardPropertyWidget, Sprite>(PropertiesPosition);

            ItemIcon.rectTransform.anchoredPosition = Vector3.zero + cardView.IconOffset; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
            
            _dataGroup.SetData(cardView.PropertyIcons);
            NameBanner.text = cardView.Id;
            DescriptionBanner.text = cardView.Wisecrack;
        }
    }
}