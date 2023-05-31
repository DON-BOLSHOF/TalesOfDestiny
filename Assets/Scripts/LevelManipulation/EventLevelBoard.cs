using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CodeAnimation;
using Model;
using Model.Data.ControllersData;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Utils.Interfaces;
using Zenject;

namespace LevelManipulation
{
    public class EventLevelBoard : MonoBehaviour, IControllerInteractionVisitor
    {
        [SerializeField] private LevelBuilder _levelBuilder;
        [SerializeField] private LevelBoardAnimations _animations;
        [SerializeField] private LevelBoardSubscriber _subscriber;
        
        private Coroutine _coroutine;

        private readonly DisposeHolder _trash = new DisposeHolder();

        private List<List<Cell>> Cells { get; set; } = new List<List<Cell>>();

        [Inject] public LocalHeroMover LocalHeroMover { get;} //Локальное изменение
        public event Action OnNextTurn;

        private void Awake()
        {
            Cells = _levelBuilder.FirstSpawn();

            LocalHeroMover.ReloadHeroPosition(Cells);
            
            SubscribeWidgets();
            _subscriber.BoundPanelsUtil(Resources
                .FindObjectsOfTypeAll<
                    AbstractStateUtil>(),GetComponentsInChildren<ISubscriber>(true).ToList());
        }

        private void SubscribeWidgets()
        {
            foreach (var cell in Cells.SelectMany(row => row))
            {
                cell.BoardItem.IClicked += widget => LocalHeroMover.BoardHeroStep(widget, Cells);
            }
        }

        private void Start()
        {
            _coroutine = StartCoroutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        public void PrepareCardsToBattle()
        {
            TakeOutCards();
            LocalHeroMover.MoveToBattlePosition();
        }

        public void ReturnCardsFromBattle()
        {
            ReturnCards();
            LocalHeroMover.MoveToCurrentPosition(Cells);
        }

        public void TakeOutCards()
        {
            StartRoutine(_animations.Exiting(CellConverter.GetItemWidgets(Cells)));
        }

        public void ReturnCards()
        {
            StartRoutine(_animations.Appearance(CellConverter.GetItemWidgets(Cells)));
        }

        public void Visit(IControllerInteraction interaction)
        {
            if ((interaction.ControllerType & ControllerInteractionType.EndJourney) == ControllerInteractionType.EndJourney)
                Reload();
        }

        private void Reload()
        {
            StartRoutine(ReloadField());
            OnNextTurn?.Invoke();
        }

        private IEnumerator ReloadField() //без анимаций код покрасивее выглядел, конечно...
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            yield return _coroutine = StartCoroutine(_animations.Exiting(CellConverter.GetItemWidgets(Cells)));

            Cells = _levelBuilder.Reload();

            SubscribeWidgets();

            LocalHeroMover.ReloadHeroPosition(Cells);

            ReturnCards();
        }

        private void StartRoutine(IEnumerator coroutine)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(coroutine);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}