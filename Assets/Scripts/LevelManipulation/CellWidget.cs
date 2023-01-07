using Widgets;

namespace LevelManipulation
{
    public class CellWidget
    {
        private BoardItemWidget _boardItem;
        private CellInfo _info;

        public BoardItemWidget BoardItem => _boardItem;
        public CellInfo Info => _info;

        public CellWidget(BoardItemWidget widget, CellInfo info)
        {
            _boardItem = widget;
            _info = info;
        }
    }
}