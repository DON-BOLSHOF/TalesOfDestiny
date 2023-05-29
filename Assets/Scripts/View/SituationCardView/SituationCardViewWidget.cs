using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.DataGroups;
using Widgets;
using Widgets.EventManagersWidgets;

namespace View.SituationCardView
{
    public class SituationCardViewWidget : CardViewWidget
    {
        [Header("Icons")]
        [SerializeField] private Image _background;
        [SerializeField] protected Image _itemIcon;
        
        [Space]
        [Header("Banners")]
        [SerializeField] protected Text _nameBanner;
        [SerializeField] protected Text _descriptionBanner;

        [Space] [Header("OffSet")]
        [SerializeField] protected float widgetOffsetCorrection; //Здесь уж точно нужен комментарий. 
                                                           //В общем, у разных иконок разные offset-ы(они в самих view лежат)
                                                           //=> widget-ы сами по себе разных размеров, так что нужна поправка
                                                           //у самого виджета
        [Space] [Header("Properties")]                                                   
        [SerializeField] private Transform _propertiesIconsPosition;
        
        public Transform PropertiesPosition => _propertiesIconsPosition;

        public Text NameBanner => _nameBanner;
        public Text DescriptionBanner => _descriptionBanner;

        private PredefinedDataGroup<CardPropertyWidget, Sprite> _dataGroup; 
        
        public override void SetViewData(CardView cardView)
        {
            _background.sprite = cardView.BackgroundView;

            var eventCardView = (SituationCardView)cardView;
            _itemIcon.sprite = eventCardView.MainView;
            _dataGroup ??= new PredefinedDataGroup<CardPropertyWidget, Sprite>(PropertiesPosition);

            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + eventCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
            _itemIcon.rectTransform.localScale = eventCardView.PortraitCorrection;

            _dataGroup.SetData(eventCardView.PropertyIcons);
            NameBanner.text = cardView.Id;
            DescriptionBanner.text = eventCardView.Wisecrack;
        }
    }
}