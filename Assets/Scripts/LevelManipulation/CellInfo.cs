using UnityEngine;

namespace LevelManipulation
{
    public class CellInfo
    {
        private Vector2Int _pos;
        public Vector2Int Position => _pos;

        private CellState _cellState = CellState.None;
        public CellState CurrentCellState => _cellState;

        private bool _isChecked;
        public bool IsChecked => _isChecked;

        public CellInfo(Vector2Int pos)
        {
            _pos = pos;
        }

        public void Check()
        {
            _isChecked = true;
        }
        public void SetDefaultState()
        {
            _isChecked = false;
            _cellState = CellState.None;
        }

        public void Visit(CellState cellState)
        {
            _cellState = cellState;
            
            if(cellState != CellState.None) Check(); //Ну вообще логично(+Нельзя сделать просто свойство от _cellState
                                                     //!= CellState.None, тк в коде напрямую будут Check позиции.)
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