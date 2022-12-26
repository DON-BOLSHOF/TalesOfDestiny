using System.Collections.Generic;
using Model.Properties;
using UnityEngine;
using Utils.Disposables;

namespace LevelManipulation
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelBuilder _levelBuilder;

        private List<List<CellWidget>> _cells = new List<List<CellWidget>>();
        private DisposeHolder _trash = new DisposeHolder();

        public ObservableProperty<Vector2> HeroPosition = new ObservableProperty<Vector2>();

        private void Start()
        {
            _cells = _levelBuilder.FirstSpawn();
            
            HeroPosition.Value = FindHeroPosition();
        }

        public void Reload()
        {
            _cells = _levelBuilder.Reload();
            HeroPosition.Value = FindHeroPosition();
        }

        private Vector2 FindHeroPosition()
        {
            Vector2 result = default;
            foreach (var row in _cells)
            {
                foreach (var cell in row)
                {
                    if (cell.Info.CurrentCellState == CellState.HeroPosition)
                    {
                        result = cell.Info.Position;
                    }
                }
            }

            return result;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}