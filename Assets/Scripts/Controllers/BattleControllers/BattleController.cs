using System;
using System.Collections.Generic;
using Cards.SituationCards.Event;
using Definitions.Creatures.Company;
using Definitions.Creatures.Enemies;
using LevelManipulation;
using Model.Data;
using Model.Data.ControllersData;
using Panels;
using UnityEngine;
using Utils;
using Utils.Disposables;
using Utils.Interfaces;
using Utils.Interfaces.Visitors;
using Zenject;

namespace Controllers.BattleControllers
{
    public class BattleController : MonoBehaviour, IControllerInteractionVisitor, IGameStateVisitor
    {
        [SerializeField] private BattleEventManager _eventManager; //Разделим обязанности
        
        [Inject] private BattleBoard _battleBoard;
        [Inject] private EventLevelBoard _eventLevelBoard;
        [Inject] private GameSession _session;

        private List<EnemyPack> _enemies;
        private List<CompanionPack> _companions;

        public List<EnemyPack> Enemies => _enemies;
        public List<CompanionPack> Companions => _companions;

        private DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _battleBoard.OnBattleEnd += EndBattle;
                return new ActionDisposable(() => _battleBoard.OnBattleEnd -= EndBattle);
            })());
        }

        private async void StartBattle()
        {
            VisitGameState(_session.GameStateAnalyzer, Stage.Start);
            
            _enemies = _eventManager.TakeEnemyPacks();//Take data
            _companions = _session.Data.CompanionsData.Companions;
            
            _eventLevelBoard.PrepareCardsField();//Prepare others
            await _eventManager.PrepareToBattle(_battleBoard);//Передаем панель на уровень ниже для синхронизации с предыдущей панелькой.
            
            _battleBoard.StartBattle(_enemies, _companions);
        }
        
        private void EndBattle(BattleEndState endState, List<CompanionPack> companionPacks)
        {
            switch (endState)
            {
                case BattleEndState.Win:
                {
                    _companions = companionPacks;
                    _eventLevelBoard.ReturnCardsField();
                    _session.Visit(this);
                    break;
                }
                case BattleEndState.Lose:
                {
                    //TODO Здесь обрисовать конец игры с точки зрения военного поражения
                    throw new NotImplementedException("Battle lose is not implemented!!!");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(endState), endState, null);
            }
            
            VisitGameState(_session.GameStateAnalyzer, Stage.End);
        }
        
        public void Visit(IControllerInteraction interaction)
        {
            if ((interaction.ControllerType & ControllerInteractionType.Battle) == ControllerInteractionType.Battle)
                StartBattle();
        }

        public void VisitGameState(GameStateAnalyzer gameStateAnalyzer, Stage stage)
        {
            gameStateAnalyzer.Visit(this, stage);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}

public enum BattleEndState
{
    Win,
    Lose
}