using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.SituationCards.Event;
using CodeAnimation;
using Model.Properties;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Utils.Interfaces;
using Widgets;
using EventType = Cards.SituationCards.Event.EventType;

namespace LevelManipulation
{
    public class EventLevelBoard : MonoBehaviour, ICustomButtonVisitor
    {
        [SerializeField] private LevelBuilder _levelBuilder;
        [SerializeField] private LevelBoardAnimations _animations;
        [SerializeField] private LevelBoardSubscriber _subscriber;

        [HideInInspector]
        public ObservableProperty<Vector2Int> HeroPosition = new ObservableProperty<Vector2Int>(); //Локальное изменение

        [HideInInspector] public ObservableProperty<Vector2>
            GlobalHeroPosition =
                new ObservableProperty<Vector2>(); //Честно не думал, что придется использовать этот атрибут

        private Coroutine _coroutine;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private List<List<CellWidget>> Cells { get; set; } = new List<List<CellWidget>>();

        public event Action OnNextTurn;

        private void Start()
        {
            Cells = _levelBuilder.FirstSpawn();

            SubscribeWidgets();
            _subscriber.BoundPanelsUtil(Resources
                .FindObjectsOfTypeAll<
                    AbstractTextPanelUtil>(),GetComponentsInChildren<ISubscriber>(true).ToList()); //Ну борд с контроллерами +- на одном уровне, так что он может о них знать

            ChangeHeroPosition(FindHeroPosition());

            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        private void Reload()
        {
            StartRoutine(ReloadCorotine());
            OnNextTurn?.Invoke();
        }

        public void StartBattle()
        {
            TakeOutCards();
        }

        public void EndBattle()
        {
            ReturnCards();
        }

        private void TakeOutCards()
        {
            StartRoutine(_animations.Exiting(CellConverter.GetItemWidgets(Cells)));
        }

        private void ReturnCards()
        {
            StartRoutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(coroutine);
        }

        private IEnumerator ReloadCorotine() //без анимаций код покрасивее выглядел, конечно...
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            yield return _coroutine = StartCoroutine(_animations.Exiting(CellConverter.GetItemWidgets(Cells)));

            Cells = _levelBuilder.Reload();

            SubscribeWidgets();

            ChangeHeroPosition(FindHeroPosition());

            ReturnCards();
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

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.EndJourney) == EventType.EndJourney)
                Reload();
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}