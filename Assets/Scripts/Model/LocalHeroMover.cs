using System.Collections.Generic;
using System.Linq;
using LevelManipulation;
using Model.Properties;
using UnityEngine;
using Widgets.BoardWidgets;
using Zenject;

namespace Model
{
    public class LocalHeroMover
    {
        [Inject] private GlobalHeroMover _globalHeroMover;
        
        public ObservableProperty<Vector2Int> BoardHeroPosition = new ObservableProperty<Vector2Int>(); //Локальное изменение

        private Vector2Int _currentPosition;

        public void ReloadHeroPosition(List<List<Cell>> cells)
        {
            ChangeHeroPosition(FindBoardHeroPosition(cells), cells);
        }
        
        private Vector2Int FindBoardHeroPosition(List<List<Cell>> cells)
        {
            Vector2Int result = default;
            foreach (var cell in from row in cells
                     from cell in row
                     where cell.Info.CurrentCellState == CellState.HeroPosition
                     select cell)
            {
                result = cell.Info.Position;
            }

            return result;
        }

        public void MoveToBattlePosition()
        {
            _globalHeroMover.MoveToBattlePosition();
        }

        public void MoveToCurrentPosition(List<List<Cell>> cells)
        {
            ChangeHeroPosition(_currentPosition, cells);
        }

            public void BoardHeroStep(BoardItemWidget widgetStep, List<List<Cell>> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            for (var j = 0; j < cells[i].Count; j++)
            {
                if (Equals(cells[i][j].BoardItem, widgetStep))
                {
                    ChangeHeroPosition(new Vector2Int(i, j), cells);
                }
            }
        }

        private void ChangeHeroPosition(Vector2Int value, List<List<Cell>> cells)
        {
            BoardHeroPosition.Value = value;
            _currentPosition = BoardHeroPosition.Value;
            _globalHeroMover.ChangeHeroPosition(cells[BoardHeroPosition.Value.x][BoardHeroPosition.Value.y].BoardItem.transform.position);
        }
    }
}