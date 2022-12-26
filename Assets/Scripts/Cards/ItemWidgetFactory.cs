using System;
using UnityEngine;
using View;
using Widgets;
using Object = UnityEngine.Object;

namespace Cards
{
    public static class ItemWidgetFactory
    {
        public static GameObject CreateInstance(GameObject prefab, Transform container, Vector2 position)
        {
            var instantiate = Object.Instantiate(prefab, container);
            instantiate.GetComponent<RectTransform>().anchoredPosition = position;

            return instantiate;
        }

        public static void FulFillItemWidget(ItemWidget cardWidget)
        {
            DeactivatePossibleView(cardWidget);

            var card = LevelCardFactory.GetLevelCardRandomly();

            ItemWidgetView cardView = default;

            switch (card.Type)
            {
                case CardType.Situation:
                {
                    cardView = Resources.Load<ItemWidgetView>("CardView/EventCardView");
                    break;
                }
                default:
                {
                    throw new ArgumentException($"Invalid type of card: {card.Type}");
                }
            }

            var instantiate = Object.Instantiate(cardView, cardWidget.transform, false);
            instantiate.transform.SetSiblingIndex(1);

            cardWidget.GetComponent<ItemWidget>().SetData(card);
        }

        private static void DeactivatePossibleView(ItemWidget cardWidget)
        {
            var activeView = cardWidget.GetComponentInChildren<ItemWidgetView>();
            if (activeView != null)
                activeView.gameObject.SetActive(false);
        }
    }
}