using System;
using System.Collections.Generic;
using Cards.SituationCards.Event;
using Definitions.Creatures.Company;
using Definitions.Creatures.Enemies;
using LevelManipulation;
using Panels;
using UnityEngine;
using Utils.Disposables;
using Zenject;
using EventType = Cards.SituationCards.Event.EventType;

namespace Controllers
{
    public class BattleController : MonoBehaviour, ICustomButtonVisitor
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
            _enemies = _eventManager.TakeEnemyPacks();//Take data
            _companions = _session.Data.CompanionsData.Companions;
            
            _eventLevelBoard.PrepareToBattle();//Prepare others
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
                    _eventLevelBoard.EndBattle();
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
        }
        
        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.Battle) == EventType.Battle)
                StartBattle();
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