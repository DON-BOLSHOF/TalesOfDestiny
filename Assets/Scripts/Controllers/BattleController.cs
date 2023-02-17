using System.Collections.Generic;
using System.Threading.Tasks;
using Cards.SituationCards.Event;
using Definitions.Enemies;
using LevelManipulation;
using UnityEngine;
using EventType = Cards.SituationCards.Event.EventType;

namespace Controllers
{
    public class BattleController : MonoBehaviour, ICustomButtonVisitor
    {
        [SerializeField] private BattleEventManager _eventManager; //Разделим обязанности

        private LevelBoard _levelBoard;

        private List<EnemyPack> _enemies;

        private void Start()
        {
            _levelBoard = FindObjectOfType<LevelBoard>();
        }

        private async void StartBattle()
        {
            _enemies = new List<EnemyPack>(_eventManager.EnemyPacks);
            _levelBoard.StartBattle();
            _eventManager.Attack();
            await Task.Delay(2000);
            
            Debug.Log("Battle Started!!!");
        }

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.Battle) == EventType.Battle)
                StartBattle();
        }
    }
}