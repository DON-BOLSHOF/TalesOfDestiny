using System;
using System.Collections.Generic;
using Cards;
using Components;
using UnityEngine;
using Utils;
using Widgets;
using Widgets.BoardWidgets;

namespace LevelManipulation
{
    [Serializable]
    public sealed class LevelBuilder
    {
        [SerializeField] private FieldFormer _fieldFormer;

        private FieldBuilder _fieldBuilder;

        private readonly List<List<BoardItemWidget>> _fieldPull = new List<List<BoardItemWidget>>();
        private Vector2Int _tableSize;

        public List<List<Cell>> FirstSpawn()
        {
            InitializeField();
            _fieldBuilder = new FieldBuilder(_tableSize);

            var fieldInfo = _fieldBuilder.FirstSpawn();
            FillField(fieldInfo);

            return CellConverter.GenerateCell(fieldInfo, _fieldPull);
        }

        public List<List<Cell>> Reload()
        {
            DeactivateLevelCard();
            var fieldInfo = _fieldBuilder.ReloadLevel();
            FillField(fieldInfo);

            return CellConverter.GenerateCell(fieldInfo, _fieldPull);
        }

        private void DeactivateLevelCard()
        {
            foreach (var row in _fieldPull)
            {
                foreach (var boardItemWidget in row)
                {
                    boardItemWidget.gameObject.SetActive(false);
                }
            }
        }

        private void FillField(List<List<CellInfo>> info)
        {
            for (var y = 0; y < info.Count; y++)
            {
                for (var x = 0; x < info[0].Count; x++)
                {
                    if ((int)info[y][x].CurrentCellState < 1) continue;
                    _fieldPull[y][x].gameObject.SetActive(true);
                    FulFillCard(info[y][x], _fieldPull[y][x], new Vector2(y, x));
                }
            }
        }

        private void InitializeField()
        {
            var field = _fieldFormer.FormField();
            foreach (var row in field)
            {
                var rowInstance = new List<BoardItemWidget>();
                foreach (var item in row)
                {
                    var instance =(BoardItemWidget)ItemWidgetFactory.CreateItemWidgetInstance(item.Prefab, _fieldFormer.Field, item.Position, WidgetType.Boarder);
                    rowInstance.Add(instance);
                }

                _fieldPull.Add(rowInstance);
            }

            _tableSize = new Vector2Int(field.Count, field[0].Count);
        }

        private void FulFillCard(CellInfo cardInfo, BoardItemWidget cardWidget, Vector2 position)
        {
            var levelCard = ItemWidgetFactory.GetLevelCardRandomlyFromDefs(cardInfo.CurrentCellState);
            ItemWidgetFactory.FulFillItemWidget(cardWidget, WidgetType.Boarder, levelCard);

            var heroBeholder = cardWidget.GetComponent<HeroPositionBeholderComponent>(); // Просто две строки в отдельную 
            heroBeholder.SetPosition(position);                                 //абстракцию выводить...
        }
    }
}