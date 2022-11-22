using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace LevelManipulation
{
    [Serializable]
    public class FieldBuilder
    {
        private List<List<CellInfo>> _field = new List<List<CellInfo>>();
        [SerializeField] private int _maxCardAmount = 10;

        private Vector2 _currentPosition;
        private Vector2 _heroPosition;
        private int _currentCardAmount;
        private float _branchPerCent; //Чтобы по одной ветке не шел
        private float CurrentSpawnPerCent => (1 - (float)_currentCardAmount / _maxCardAmount) * 100;

        private Vector2 _tableSize;

        public FieldBuilder(Vector2 tableSize)
        {
            _tableSize = tableSize;
            
            _field = Enumerable.Range(0, (int)_tableSize.x)
                .Select(f => Enumerable.Range(0, (int)_tableSize.y).Select(fa => new CellInfo(new Vector2(f, fa))).ToList()).ToList();
        }

        public List<List<CellInfo>> FirstSpawn()
        {
            PositionBuilder();
            return SpawnLevel();
        }

        private List<List<CellInfo>> ReloadLevel()
        {
            _currentCardAmount = 0;
            _field.ForEach(delegate(List<CellInfo> list) { list.ForEach(delegate(CellInfo field)
            {
                field.SetDefaultState(); }); });
            _currentPosition = _heroPosition = new Vector2((int)_tableSize.x / 2, (int) _tableSize.y/2);
            _field[(int)_currentPosition.x][(int)_currentPosition.y].Visit(CellState.HeroPosition);
            return SpawnLevel();
        }

        private List<List<CellInfo>> SpawnLevel()
        {
            while (CurrentSpawnPerCent > 0)
            {
                var around = AroundPlace(_currentPosition);

                if (_currentPosition == _heroPosition && around == AroundPosition.None)
                    break;

                if ((around & AroundPosition.Left) != 0)
                {
                    if (FieldStep(_currentPosition + new Vector2(0, -1)))
                    {
                        continue;
                    }
                }

                if ((around & AroundPosition.Top) != 0)
                {
                    if (FieldStep(_currentPosition + new Vector2(-1, 0)))
                    {
                        continue;
                    }
                }

                if ((around & AroundPosition.Right) != 0)
                {
                    if (FieldStep(_currentPosition + new Vector2(0, 1)))
                    {
                        continue;
                    }
                }

                if ((around & AroundPosition.Bottom) != 0)
                {
                    if (FieldStep(_currentPosition + new Vector2(1, 0)))
                    {
                        continue;
                    }
                }

                if (_field[(int)_currentPosition.x][(int)_currentPosition.y].CurrentCellState != CellState.HeroPosition)
                    _field[(int)_currentPosition.x][(int)_currentPosition.y].Visit(CellState.EndJourneyPosition);

                _currentPosition = _heroPosition;
                _branchPerCent = 0f;
            }

            return _field;
        }

        private bool FieldStep(Vector2 pos)
        {
            if (Probability(CurrentSpawnPerCent - _branchPerCent))
            {
                _field[(int)pos.x][(int)pos.y].Visit(CellState.CardPosition);
                _currentCardAmount++;
                _branchPerCent += 5;
                _currentPosition = pos;
                return true;
            }

            _field[(int)pos.x][(int)pos.y].Visit(CellState.Checked);
            return false;
        }

        private AroundPosition AroundPlace(Vector2 pos)
        {
            AroundPosition result = AroundPosition.None;

            var leftPos = IsInTable(new Vector2(pos.x, pos.y - 1)) &&
                          _field[(int)(pos.x)][(int)pos.y - 1].CurrentCellState == CellState.UnChecked;
            if (leftPos)
                result |= AroundPosition.Left;

            var topPos = IsInTable(new Vector2(pos.x - 1, pos.y)) && //Y сверху вниз идет
                         _field[(int)(pos.x - 1)][(int)pos.y].CurrentCellState == CellState.UnChecked;
            if (topPos)
                result |= AroundPosition.Top;

            var rightPos = IsInTable(new Vector2(pos.x, pos.y + 1)) &&
                           _field[(int)(pos.x)][(int)pos.y + 1].CurrentCellState == CellState.UnChecked;
            if (rightPos)
                result |= AroundPosition.Right;

            var bottomPos = IsInTable(new Vector2(pos.x + 1, pos.y)) &&
                            _field[(int)(pos.x + 1)][(int)pos.y].CurrentCellState == CellState.UnChecked;
            if (bottomPos)
                result |= AroundPosition.Bottom;

            if (result > 0)
                result -= AroundPosition.None;

            return result;
        }

        [Flags]
        private enum AroundPosition
        {
            None = 0,
            Left = 1,
            Top = 2,
            Right = 4,
            Bottom = 8
        }

        private bool IsInTable(Vector2 pos)
        {
            if (pos.x >= _tableSize.x || pos.x < 0)
                return false;

            if (pos.y >= _tableSize.y || pos.y < 0)
                return false;

            return true;
        }

        private void PositionBuilder()
        {
            int iterator = 0;
            int threshold = (_field.Count + _field[0].Count - 2) * 2;
            while (iterator < threshold)
            {
                for (int x = 0; x < _field[0].Count; x++)
                {
                    if (Probability(iterator * 100 / threshold))
                    {
                        _field[0][x].Visit(CellState.HeroPosition);
                        _heroPosition = _field[0][x].Position;
                        _currentPosition = _heroPosition;
                        return;
                    }

                    iterator++;
                }

                for (int y = 1; y < _field.Count; y++)
                {
                    if (Probability(iterator * 100 / threshold))
                    {
                        _field[y][_field[0].Count - 1].Visit(CellState.HeroPosition);
                        _heroPosition = _field[y][_field[0].Count - 1].Position;
                        _currentPosition = _heroPosition;
                        return;
                    }

                    iterator++;
                }

                for (int x = _field[0].Count - 2; x >= 0; x--)
                {
                    if (Probability(iterator * 100 / threshold))
                    {
                        _field[_field.Count - 1][x].Visit(CellState.HeroPosition);
                        _heroPosition = _field[_field.Count - 1][x].Position;
                        _currentPosition = _heroPosition;
                        return;
                    }

                    iterator++;
                }

                for (int y = _field.Count - 2; y > 0; y--)
                {
                    if (Probability(iterator * 100 / threshold))
                    {
                        _field[y][0].Visit(CellState.HeroPosition);
                        _heroPosition = _field[y][0].Position;
                        _currentPosition = _heroPosition;
                        return;
                    }

                    iterator++;
                }
            }
        }

        private bool Probability(float percent)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            int res = rand.Next(101);
            if (res <= percent) return true;

            return false;
        }
    }

    public enum AroundPosition
    {
        None,
        Left,
        Top,
        Right,
        Bottom
    }
}
