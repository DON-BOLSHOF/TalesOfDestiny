using System.Collections.Generic;
using System.Linq;
using Definitions.Creatures.Enemies;
using UnityEngine;

namespace Panels
{
    public class BattleBoard : AbstractPanelUtil
    {
        [SerializeField] private Animator _animator;
        
        [SerializeField] private CreaturePanel _enemyPanel;
        [SerializeField] private CreaturePanel _heroAllyPanel;
        [SerializeField] private CrowdPanel _crowdPanel;

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

        public void StartBattle(IList<EnemyPack> enemies)
        {
            _enemyPanel.DynamicInitialization(enemies.Take(5));
            _heroAllyPanel.DynamicInitialization(enemies.Take(5));

            if(enemies.Count > 5)
                _crowdPanel.Activate(enemies.Skip(5).ToList());
            
            Show();
        }
    }
}