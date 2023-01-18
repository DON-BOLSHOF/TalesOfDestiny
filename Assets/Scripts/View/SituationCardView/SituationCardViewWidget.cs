using UnityEngine;
using UnityEngine.UI;
using Widgets;

namespace View.SituationCardView
{
    public class SituationCardViewWidget : ItemWidgetView
    {
        [Header("Icons")]
        [SerializeField] private Transform _propertiesIconsPosition;
        [SerializeField] private Image _itemIcon;
        
        [Space]
        [Header("Banners")]
        [SerializeField] private Text _nameBanner;
        [SerializeField] private Text _descriptionBanner;

        [Space] [Header("OffSet")]
        [SerializeField] private float widgetOffsetCorrection; //Здесь уж точно нужен комментарий. 
                                                           //В общем, у разных иконок разные offset-ы(они в самих view лежат)
                                                           //=> widget-ы сами по себе разных размеров, так что нужна поправка
                                                           //у самого виджета   
        public Transform PropertiesPosition => _propertiesIconsPosition;

        public Text NameBanner => _nameBanner;
        public Text DescriptionBanner => _descriptionBanner;

        private PredefinedDataGroup<CardPropertyWidget, Sprite> _dataGroup; 
        
        public override void SetViewData(CardView cardView)
        {
            base.SetViewData(cardView);
            var eventCardView = (SituationCardView)cardView;
            _itemIcon.sprite = eventCardView.MainView;
            _dataGroup ??= new PredefinedDataGroup<CardPropertyWidget, Sprite>(PropertiesPosition);

            _itemIcon.rectTransform.anchoredPosition = Vector3.zero + eventCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
            
            _dataGroup.SetData(eventCardView.PropertyIcons);
            NameBanner.text = cardView.Id;
            DescriptionBanner.text = eventCardView.Wisecrack;
        }
    }
}