using System.Collections.Generic;
using System.Linq;
using Definitions.Creatures;
using Definitions.Creatures.Enemies;
using UnityEngine;

namespace Panels
{
    public class BattleBoard : AbstractPanelUtil
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private CreaturePanel _enemyPanel;
        [SerializeField] private CreaturePanel _heroAllyPanel;

        public override void Show()
        {
            OnChangeState?.Invoke(true);
            
            _enemyPanel.Show();
            _heroAllyPanel.Show();
        }

        public override void Exit()
        {
            _enemyPanel.Exit();
            _heroAllyPanel.Exit();
            
            OnChangeState?.Invoke(false);
        }

        public void StartBattle(IEnumerable<EnemyPack> enemies)
        {
            _enemyPanel.DynamicInitialization(enemies);
            _heroAllyPanel.DynamicInitialization(enemies);
            
            Show();
        }
    }
}