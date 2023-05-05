using Widgets.BoardWidgets;

namespace LevelManipulation
{
    public class Cell
    {
        private BoardItemWidget _boardItem;
        private CellInfo _info;

        public BoardItemWidget BoardItem => _boardItem;
        public CellInfo Info => _info;

        public Cell(BoardItemWidget widget, CellInfo info)
        {
            _boardItem = widget;
            _info = info;
        }
    }
}