using System.Collections.Generic;
using Cards;
using UnityEngine;
using Widgets;

namespace LevelManipulation
{
    public sealed class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private FieldFormer _fieldFormer;
        [SerializeField] private FieldBuilder _fieldBuilder;

        private readonly List<List<GameObject>> _fieldPull = new List<List<GameObject>>();
        private Vector2 _tableSize;
        
        private void Awake()
        {
            InitializeField();
        }

        private void Start()
        {
            _fieldBuilder = new FieldBuilder(_tableSize);
            var fieldInfo = _fieldBuilder.FirstSpawn();
            FillField(fieldInfo);
        }

        private void FillField(List<List<CellInfo>> info)
        {
            for (var y = 0; y < info.Count; y++)
            {
                for (var x = 0; x < info[0].Count; x++)
                {
                    if ((int)info[y][x].CurrentCellState <= 1) continue;
                    _fieldPull[y][x].SetActive(true);
                    FulFillCard(_fieldPull[y][x]);
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

        private void FulFillCard(GameObject go)
        {
            var cardWidget = go.GetComponent<ItemWidget>();
            
            ItemWidgetFactory.FulFillItemWidget(cardWidget);
        }
    }
}
