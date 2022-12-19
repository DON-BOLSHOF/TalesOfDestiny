using System;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManipulation
{
    [Serializable]
    
    public class FieldFormer
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private RectTransform _field;

        [SerializeField] private int _prefabOffset = 25;
        public RectTransform Field => _field;

        private List<List<FieldItem>> _fieldItems = new List<List<FieldItem>>();

        public List<List<FieldItem>> FormField()
        {
            var sizTransform = _prefab.GetComponent<RectTransform>();
            var prefabSize = sizTransform.sizeDelta;

            var sizeDelta = _field.sizeDelta;
            var newX = (int)sizeDelta.x / (int)(prefabSize.x+_prefabOffset);
            var newY = (int)sizeDelta.y / (int)(prefabSize.y+_prefabOffset);

            var neededMatrix = new Vector2(newX, newY);
            for (int i = 0; i < neededMatrix.y; i++)
            {
                List<FieldItem> row = new List<FieldItem>();
                
                for (int j = 0; j < neededMatrix.x; j++)
                {
                    var delta = new Vector2(j * (prefabSize.x + _prefabOffset), -i* (prefabSize.y+_prefabOffset));
                    var pos = _field.GetComponent<RectTransform>().anchoredPosition + delta;

                    var item = new FieldItem(_prefab, pos);
                    row.Add(item);
                }
                
                _fieldItems.Add(row);
            }

            return _fieldItems;
        }
        
        public class FieldItem
        {
            public GameObject Prefab { get; }

            public Vector2 Position { get; }

            public FieldItem(GameObject prefab, Vector2 pos)
            {
                Prefab = prefab;
                Position = pos;
            }
        }
    }
}