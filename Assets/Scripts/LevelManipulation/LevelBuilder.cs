using System;
using System.Collections.Generic;
using Cards;
using Components;
using UnityEngine;
using Widgets;

namespace LevelManipulation
{
    [Serializable]
    public sealed class LevelBuilder
    {
        [SerializeField] private FieldFormer _fieldFormer;

        private FieldBuilder _fieldBuilder;

        private readonly List<List<GameObject>> _fieldPull = new List<List<GameObject>>();
        private Vector2 _tableSize;

        public List<List<CellWidget>> FirstSpawn()
        {
            InitializeField();
            _fieldBuilder = new FieldBuilder(_tableSize);

            var fieldInfo = _fieldBuilder.FirstSpawn();
            FillField(fieldInfo);

            return GenerateCellWidgets(fieldInfo, _fieldPull);
        }

        public List<List<CellWidget>> Reload()
        {
            var fieldInfo = _fieldBuilder.ReloadLevel();
            FillField(fieldInfo);

            return GenerateCellWidgets(fieldInfo, _fieldPull);
        }

        private void FillField(List<List<CellInfo>> info)
        {
            for (var y = 0; y < info.Count; y++)
            {
                for (var x = 0; x < info[0].Count; x++)
                {
                    if ((int)info[y][x].CurrentCellState < 1) continue;
                    _fieldPull[y][x].SetActive(true);
                    FulFillCard(_fieldPull[y][x], new Vector2(y, x));
                }
            }
        }

        private void InitializeField()
        {
            var field = _fieldFormer.FormField();
            foreach (var rows in field)
            {
                var rowInstance = new List<GameObject>();
                foreach (var row in rows)
                {
                    var instantiate = ItemWidgetFactory.CreateInstance(row.Prefab, _fieldFormer.Field, row.Position);
                    rowInstance.Add(instantiate);
                }

                _fieldPull.Add(rowInstance);
            }

            _tableSize = new Vector2(field.Count, field[0].Count);
        }

        private void FulFillCard(GameObject go, Vector2 position)
        {
            var cardWidget = go.GetComponent<ItemWidget>();

            ItemWidgetFactory.FulFillItemWidget(cardWidget);

            var heroBeholder = go.GetComponent<HeroPositionBeholderComponent>(); // Просто две строки в отдельную 
            heroBeholder.SetPosition(position);                                 //абстракцию выводить...
        }

        private List<List<CellWidget>> GenerateCellWidgets(List<List<CellInfo>> cellInfos, List<List<GameObject>> items)
        {
            List<List<CellWidget>> list = new List<List<CellWidget>>();

            if (cellInfos.Count != items.Count || cellInfos[0].Count != items[0].Count)
                throw new ArgumentException("items and cells asynchronous");

            for (int i = 0; i < cellInfos.Count; i++)
            {
                var row = new List<CellWidget>();
                for (int j = 0; j < cellInfos[0].Count; j++)
                {
                    var cellWidget = new CellWidget(items[i][j], cellInfos[i][j]);
                    row.Add(cellWidget);
                }

                list.Add(row);
            }

            return list;
        }
    }
}