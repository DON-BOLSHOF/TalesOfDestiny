using System;
using System.Threading.Tasks;
using Panels;
using UnityEngine;

namespace Model
{
    [Serializable]
    public class FightMechanism
    {
        [SerializeField] private float attackDelay;

        private bool _enable;

        public async void StartBattle(CreaturePanel enemyPanel, CreaturePanel heroAllyPanel)
        {
            _enable = true;
            var i = 0;

            while (_enable && enemyPanel.ActiveCreaturesAmount > 0 && heroAllyPanel.ActiveCreaturesAmount > 0)
            {
                if (i % 2 == 0)
                {
                    await enemyPanel.ForceToAttack(heroAllyPanel.RandomCreature);
                    i = 1;
                }
                else
                {
                    await heroAllyPanel.ForceToAttack(enemyPanel.RandomCreature);
                    i = 0;
                }
                await Task.Delay((int)(attackDelay*1000));
            }
        }

        public void StopBattle()
        {
            _enable = false;
        }
    }
}