using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeAnimation;
using Model.Properties;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Widgets;

namespace LevelManipulation
{
    public class LevelBoard : MonoBehaviour
    {
        [SerializeField] private LevelBuilder _levelBuilder;

        private List<List<CellWidget>> _cells = new List<List<CellWidget>>();
        public ObservableProperty<Vector2Int> HeroPosition = new ObservableProperty<Vector2Int>();

        private LevelBoardAnimations _animations = new LevelBoardAnimations();
        private Coroutine _coroutine; 
            
        private readonly DisposeHolder _trash = new DisposeHolder();

        public List<List<CellWidget>> Cells => _cells;


        private void Start()
        {
            _cells = _levelBuilder.FirstSpawn();

            SubscribeWidgets();
            
            HeroPosition.Value = FindHeroPosition();

            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(_cells)));
        }

        public void Reload()
        {
            StartCoroutine(ReloadCorotine());
        }

        private IEnumerator ReloadCorotine() //без анимаций код покрасивее выглядел, конечно...
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            yield return _animations.Exiting(CellConverter.GetItemWidgets(_cells));
            
            _cells = _levelBuilder.Reload();

            SubscribeWidgets();

            HeroPosition.Value = FindHeroPosition();
            
            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(_cells)));
        }
        
        private Vector2Int FindHeroPosition()
        {
            Vector2Int result = default;
            foreach (var cell in from row in _cells
                     from cell in row
                     where cell.Info.CurrentCellState == CellState.HeroPosition
                     select cell)
            {
                result = cell.Info.Position;
            }

            return result;
        }

        private void SubscribeWidgets()
        {
            foreach (var row in _cells)
            {
                foreach (var cell in row)
                {
                    cell.BoardItem.IClicked += HeroStep;
                }
            }
        }

        private void HeroStep(BoardItemWidget widgetStep)
        {
            for (var i = 0; i < _cells.Count; i++)
            for (var j = 0; j < _cells[i].Count; j++)
            {
                if (Equals(_cells[i][j].BoardItem, widgetStep))
                    HeroPosition.Value = new Vector2Int(i, j);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}