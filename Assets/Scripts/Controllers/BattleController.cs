using System.Collections.Generic;
using Cards.SituationCards.Event;
using Definitions.Enemies;
using LevelManipulation;
using Panels;
using UnityEngine;
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

        private async void StartBattle()
        {
            _enemies = _eventManager.TakeEnemyPacks();
            _eventLevelBoard.StartBattle();
            await _eventManager.PrepareToBattle(_battleBoard);//Передаем панель на уровень ниже для синхронизации с предыдущей панелькой.
            
            Debug.Log("Battle Started!!!");
        }

        
        [ContextMenu("Exit")]
        public void EndBattle()
        {
            _eventLevelBoard.EndBattle();
            _battleBoard.Exit();
        }

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.Battle) == EventType.Battle)
                StartBattle();
        }
    }
}