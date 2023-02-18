using System.Collections.Generic;
using System.Threading.Tasks;
using Cards.SituationCards.Event;
using Definitions.Enemies;
using LevelManipulation;
using Panels;
using UnityEngine;
using EventType = Cards.SituationCards.Event.EventType;

namespace Controllers
{
    public class BattleController : MonoBehaviour, ICustomButtonVisitor
    {
        [SerializeField] private BattleEventManager _eventManager; //Разделим обязанности
        [SerializeField] private BattlePanel _battlePanel;

        private LevelBoard _levelBoard;

        private List<EnemyPack> _enemies;

        private void Start()
        {
            _levelBoard = FindObjectOfType<LevelBoard>();
        }

        private async void StartBattle()
        {
            _enemies = _eventManager.TakeEnemyPacks();
            _levelBoard.StartBattle();
            await _eventManager.PrepareToBattle(_battlePanel);//Передаем панель на уровень ниже для синхронизации с предыдущей панелькой.
            
            Debug.Log("Battle Started!!!");
        }

        
        [ContextMenu("Exit")]
        public void EndBattle()
        {
            _levelBoard.EndBattle();
            _battlePanel.Exit();
        }

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.Battle) == EventType.Battle)
                StartBattle();
        }
    }
}