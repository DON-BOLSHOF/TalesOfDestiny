using UnityEngine;

namespace LevelManipulation
{
    public class CellWidget
    {
        private GameObject _item;
        private CellInfo _info;

        public GameObject Item => _item;
        public CellInfo Info => _info;

        public CellWidget(GameObject widget, CellInfo info)
        {
            _item = widget;
            _info = info;
        }
    }
}