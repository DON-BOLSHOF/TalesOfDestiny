using System;
using LevelManipulation;
using UnityEngine;
using View;
using Widgets.BoardWidgets;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Cards
{
    public class ItemWidgetFactory : IFactory<Object, ItemWidget>
    {
        private readonly DiContainer _diContainer;

        [Inject]
        public ItemWidgetFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public ItemWidget Create(Object prefab)
        {
            return _diContainer.InstantiatePrefabForComponent<ItemWidget>(prefab);
        }

        private ItemWidget CreateInstance(GameObject prefab, Transform container, Vector2 position)
        {
            var instantiate = _diContainer.InstantiatePrefabForComponent<ItemWidget>(prefab, container);
            instantiate.GetComponent<RectTransform>().anchoredPosition = position;

            return instantiate;
        }

        public ItemWidget CreateItemWidgetInstance(GameObject prefab, Transform container, Vector2 position,
            WidgetType type = WidgetType.None)
        {
            var go = CreateInstance(prefab, container, position);

            ItemWidget widget =
                type switch //Здесь я гарантирую взятие правильного виджета, а в коде потом явно преобразую в нужный
                {
                    WidgetType.None => go.GetComponent<ItemWidget>(),
                    WidgetType.Boarder => go.GetComponent<BoardItemWidget>(),
                    WidgetType.PanelUtil => go.GetComponent<PanelUtilItemWidget>(),
                    _ => null
                };

            if (widget == null) throw new ArgumentException($"It is not a prefab of itemWidget or {type} was wrong!");

            return widget;
        }

        public LevelCard GetLevelCardRandomlyFromDefs(LevelCardType type)
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

        public LevelCard GetLevelCardRandomlyFromDefs(CellState type)
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

        public void FulFillItemWidget(ItemWidget cardWidget, WidgetType widgetType,
            LevelCard card)
        {
            var path = GetPath(widgetType, card.LevelCardType);

            DeactivatePossibleView(cardWidget);

            var cardView = Resources.Load<CardViewWidget>(path);
            if (cardView == null) throw new ArgumentException($"Invalid path: {path}");

            var instantiate = Object.Instantiate(cardView, cardWidget.transform, false);
            instantiate.transform.SetSiblingIndex(1);

            cardWidget.GetComponent<ItemWidget>().SetData(card);
        }

        private string GetPath(WidgetType widgetType, LevelCardType cardType)
        {
            string path = null;

            path += widgetType switch
            {
                WidgetType.Boarder => "Boarder",
                WidgetType.PanelUtil => "PanelUtil",
                _ => throw new ArgumentOutOfRangeException(nameof(widgetType), widgetType, null)
            };

            path += "CardView/";

            path += cardType switch
            {
                LevelCardType.Situation => "SituationCardView",
                LevelCardType.EndJourney => "EndJourneyCardView",
                LevelCardType.HeroPosition => "HeroCardView",
                LevelCardType.Battle => "BattleCardView",
                _ => throw new ArgumentOutOfRangeException(nameof(cardType), cardType, null)
            };

            return path;
        }

        private void DeactivatePossibleView(ItemWidget cardWidget)
        {
            var activeView = cardWidget.GetComponentInChildren<CardViewWidget>();
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