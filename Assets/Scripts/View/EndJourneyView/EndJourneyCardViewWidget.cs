using UnityEngine;
using UnityEngine.UI;

namespace View.EndJourneyView
{
    public sealed class EndJourneyCardViewWidget : ItemWidgetView
    {
        [Space]
        [Header("Banners")]
        [SerializeField] private Text _nameBanner;
        [SerializeField] private Text _descriptionBanner;

        [Space] [Header("OffSet")]
        [SerializeField] private float widgetOffsetCorrection; //Здесь уж точно нужен комментарий. 
                                                                //В общем, у разных иконок разные offset-ы(они в самих view лежат)
                                                                //=> widget-ы сами по себе разных размеров, так что нужна поправка
                                                                //у самого виджета
                                                                
         public Text NameBanner => _nameBanner;
         public Text DescriptionBanner => _descriptionBanner;
         
         public override void SetViewData(CardView cardView)
         {
             base.SetViewData(cardView);
             var eventCardView = (EndJourneyCardView)cardView;

             ItemIcon.rectTransform.anchoredPosition = Vector3.zero + eventCardView.IconOffset * widgetOffsetCorrection ; // Ну по идее это так же относится к дате, так что вроде нормально сюда вставлять
            
             NameBanner.text = cardView.Id;
             DescriptionBanner.text = eventCardView.Wisecrack;
         }
    }
}