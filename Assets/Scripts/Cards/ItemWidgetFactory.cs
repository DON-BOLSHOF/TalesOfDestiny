using System;
using Cards.SituationCards.PhysicalCard;
using LevelManipulation;
using UnityEngine;
using View;
using Widgets;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

        public static ItemWidget CreateItemWidgetInstance(GameObject prefab, Transform container, Vector2 position,
            WidgetType type = WidgetType.None)
        {
            var go = CreateInstance(prefab, container, position);

            ItemWidget widget = null;

            switch (type) //Здесь я гарантирую взятие правильного виджета, а в коде потом явно преобразую в нужный
            {
                case WidgetType.None:
                {
                    widget = go.GetComponent<ItemWidget>();
                    break;
                }
                case WidgetType.Boarder:
                {
                    widget = go.GetComponent<BoardItemWidget>();
                    break;
                }
                case WidgetType.PanelUtil:
                {
                    widget = go.GetComponent<PanelUtilItemWidget>();
                    break;
                }
            }

            if (widget == null) throw new ArgumentException($"It is not a prefab of itemWidget or {type} was wrong!");

            return widget;
        }

        public static LevelCard GetLevelCardRandomly()
        {
            return LevelCardFactory.GetLevelCardRandomly();
        }

        public static LevelCard GetLevelCardRandomlyFromDefs(LevelCardType type)
        {
            LevelCard card;
            switch (type)
            {
                case LevelCardType.Situation:
                case LevelCardType.EndJourney:
                case LevelCardType.Battle:
                case LevelCardType.HeroPosition:
                {
                    card = LevelCardFactory.GetFromListRandomly(type);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return card;
        }

        public static LevelCard GetLevelCardRandomlyFromDefs(CellState type)
        {
            LevelCard card = null;
            switch (type)
            {
                case CellState.EndJourneyPosition:
                {
                    card = GetLevelCardRandomlyFromDefs(LevelCardType.EndJourney);
                    break;
                }
                case CellState.CardPosition:
                {
                    var cardType = (LevelCardType)Random.Range((int)LevelCardType.Situation, (int)LevelCardType.Battle+1);
                    card = GetLevelCardRandomlyFromDefs(cardType);
                    break;
                }
                case CellState.HeroPosition:
                {
                    card = GetLevelCardRandomlyFromDefs(LevelCardType.HeroPosition);
                    break;
                }
                case CellState.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return card;
        }

        public static void FulFillItemWidget(ItemWidget cardWidget, WidgetType widgetType,
            LevelCard card)
        {
            var path = GetPath(widgetType, card.LevelCardType);

            DeactivatePossibleView(cardWidget);

            var cardView = Resources.Load<ItemWidgetView>(path);
            if (cardView == null) throw new ArgumentException($"Invalid path: {path}");

            var instantiate = Object.Instantiate(cardView, cardWidget.transform, false);
            instantiate.transform.SetSiblingIndex(1);

            cardWidget.GetComponent<ItemWidget>().SetData(card);
        }

        private static string GetPath(WidgetType widgetType, LevelCardType cardType)
        {
            string path = null;

            switch (widgetType)
            {
                case WidgetType.Boarder:
                    path += "Boarder";
                    break;
                case WidgetType.PanelUtil:
                    path += "PanelUtil";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(widgetType), widgetType, null);
            }

            path += "CardView/";

            switch (cardType)
            {
                case LevelCardType.Situation:
                    path+="SituationCardView";
                    break;
                case LevelCardType.EndJourney:
                    path+="EndJourneyCardView";
                    break;
                case LevelCardType.HeroPosition:
                    path += "HeroCardView";
                    break;
                case LevelCardType.Battle:
                    path += "BattleCardView";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null);
            }

            return path;
        }

        private static void DeactivatePossibleView(ItemWidget cardWidget)
        {
            var activeView = cardWidget.GetComponentInChildren<ItemWidgetView>();
            if (activeView != null)
                activeView.gameObject.SetActive(false);
        }
    }

    public enum WidgetType
    {
        None,
        Boarder,
        PanelUtil
    }
}