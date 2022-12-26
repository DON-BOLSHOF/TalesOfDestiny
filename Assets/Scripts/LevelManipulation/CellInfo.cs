using UnityEngine;

namespace LevelManipulation
{
    public class CellInfo
    {
        private Vector2 _pos;
        public Vector2 Position => _pos;
        

        private CellState _cellState = CellState.None;
        public CellState CurrentCellState => _cellState;

        private bool _isChecked;
        public bool IsChecked => _isChecked;

        public CellInfo(Vector2 pos)
        {
            _pos = pos;
        }

        public void Check()
        {
            _isChecked = true;
        }
        public void SetDefaultState()
        {
            _cellState = CellState.None;
        }

        public void Visit(CellState cellState)
        {
            _cellState = cellState;
        }
    }

    public enum CellState
    {
        None,
        CardPosition,
        HeroPosition,
        EndJourneyPosition
    }
}