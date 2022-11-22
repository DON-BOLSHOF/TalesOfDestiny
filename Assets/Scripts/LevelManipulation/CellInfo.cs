using UnityEngine;

namespace LevelManipulation
{
    public class CellInfo
    {
        public CellInfo(Vector2 pos)
        {
            _pos = pos;
        }

        private Vector2 _pos;
        public Vector2 Position => _pos;

        private CellState _cellState = CellState.UnChecked;
        public CellState CurrentCellState => _cellState;

        public void SetDefaultState()
        {
            _cellState = CellState.UnChecked;
        }

        public void Visit(CellState cellState)
        {
            _cellState = cellState;
        }
    }

    public enum CellState
    {
        UnChecked,
        Checked,
        CardPosition,
        HeroPosition,
        EndJourneyPosition
    }
}