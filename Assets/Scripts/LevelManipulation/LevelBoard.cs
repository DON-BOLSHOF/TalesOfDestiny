using System;
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

        [HideInInspector] public ObservableProperty<Vector2Int> HeroPosition = new ObservableProperty<Vector2Int>(); //Локальное изменение
        [HideInInspector] public ObservableProperty<Vector2> GlobalHeroPosition = new ObservableProperty<Vector2>(); //Честно не думал, что придется использовать этот атрибут

        private LevelBoardAnimations _animations = new LevelBoardAnimations();
        private Coroutine _coroutine;

        private readonly DisposeHolder _trash = new DisposeHolder();

        public List<List<CellWidget>> Cells { get; private set; } = new List<List<CellWidget>>();

        public event Action OnNextTurn;

        private void Start()
        {
            Cells = _levelBuilder.FirstSpawn();

            SubscribeWidgets();

            ChangeHeroPosition(FindHeroPosition());

            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        public void Reload()
        {
            StartCoroutine(ReloadCorotine());
            OnNextTurn?.Invoke();
        }

        private IEnumerator ReloadCorotine() //без анимаций код покрасивее выглядел, конечно...
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            yield return _animations.Exiting(CellConverter.GetItemWidgets(Cells));

            Cells = _levelBuilder.Reload();

            SubscribeWidgets();

            ChangeHeroPosition(FindHeroPosition());
            
            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        private Vector2Int FindHeroPosition()
        {
            Vector2Int result = default;
            foreach (var cell in from row in Cells
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
            foreach (var row in Cells)
            {
                foreach (var cell in row)
                {
                    cell.BoardItem.IClicked += HeroStep;
                }
            }
        }

        private void HeroStep(BoardItemWidget widgetStep)
        {
            for (var i = 0; i < Cells.Count; i++)
            for (var j = 0; j < Cells[i].Count; j++)
            {
                if (Equals(Cells[i][j].BoardItem, widgetStep))
                {
                    ChangeHeroPosition(new Vector2Int(i, j));
                }
            }
        }

        private void ChangeHeroPosition(Vector2Int value)
        {
            HeroPosition.Value = value;
            GlobalHeroPosition.Value = Cells[HeroPosition.Value.x][HeroPosition.Value.y].BoardItem.transform.position;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}