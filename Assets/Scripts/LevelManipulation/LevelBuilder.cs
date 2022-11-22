using System.Collections.Generic;
using Cards;
using UnityEngine;
using Widgets;

namespace LevelManipulation
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private FieldFormer _fieldFormer;
        [SerializeField] private FieldBuilder _fieldBuilder;

        private readonly List<List<GameObject>> _field = new List<List<GameObject>>();
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
                    _field[y][x].SetActive(true);
                    FulFillCard(_field[y][x]);
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
                    var instantiate = Instantiate(row.Prefab, _fieldFormer.Field);
                    instantiate.GetComponent<RectTransform>().anchoredPosition = row.Position;
                    rowInstance.Add(instantiate);
                }
                
                _field.Add(rowInstance);
            }

            _tableSize = new Vector2(field.Count, field[0].Count);
        }

        private void FulFillCard(GameObject go)
        {
            var card = LevelCardFactory.GetLevelCardRandomly(); 
            go.GetComponent<ItemWidget>().SetData(card);
        }
    }
}
