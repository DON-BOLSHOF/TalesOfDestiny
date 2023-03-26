using System;
using System.Collections.Generic;
using Cards.SituationCards.Event;
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

        private List<EnemyPack> _enemies;

        private DisposeHolder _trash = new DisposeHolder();

        private void Start()
        {
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _battleBoard.OnChangeState += EndBattle;
                return new ActionDisposable(() => _battleBoard.OnChangeState -= EndBattle);
            })());
        }

        private async void StartBattle()
        {
            _enemies = _eventManager.TakeEnemyPacks();
            _eventLevelBoard.PrepareToBattle();
            await _eventManager.PrepareToBattle(_battleBoard);//Передаем панель на уровень ниже для синхронизации с предыдущей панелькой.
            
            _battleBoard.StartBattle(_enemies);
        }
        
        private void EndBattle(bool value)
        {
            if(value) return;
            
            _eventLevelBoard.EndBattle();
            Debug.Log("End of Battle");
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