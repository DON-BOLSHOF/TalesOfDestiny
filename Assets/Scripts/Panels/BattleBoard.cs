using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Definitions.Creatures.Company;
using Definitions.Creatures.Enemies;
using Model;
using UnityEngine;
using Utils.Disposables;

namespace Panels
{
    public class BattleBoard : AbstractStateUtil
    {
        [SerializeField] private CreaturePanel _enemyPanel;
        [SerializeField] private CreaturePanel _companyPanel;
        [SerializeField] private CrowdPanel _crowdPanel;

        [SerializeField] private FightMechanism _fightMechanism;
        [SerializeField] private int _spawnDelay = 50;

        private DisposeHolder _trash = new DisposeHolder();

        public event Action<BattleEndState, List<CompanionPack>> OnBattleEnd;

        private void Awake()
        {
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _companyPanel.OnExit += Exit;
                return new ActionDisposable(() => _companyPanel.OnExit -= Exit);
            })());
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _enemyPanel.OnExit += Exit;
                return new ActionDisposable(() => _enemyPanel.OnExit -= Exit);
            })());
        }

        private async Task Show() //Нарушил принцип Лисков!
        {
            OnChangeState?.Invoke(true);

            await _enemyPanel.Show();
            await _companyPanel.Show();
        }

        private async void Exit()
        {
            _fightMechanism.StopBattle();

            var enemies = _enemyPanel.TakeActivePacks();
            var companions = _companyPanel.TakeActivePacks();
            var endState = enemies.Count>0? BattleEndState.Lose: BattleEndState.Win;
            OnBattleEnd?.Invoke(endState, Array.ConvertAll(companions.ToArray(), creature => (CompanionPack)creature).ToList());

            var enemyDeactivate = _enemyPanel.Deactivate();
            var companyDeactivate = _companyPanel.Deactivate();
            await Task.WhenAll(enemyDeactivate, companyDeactivate);

            OnChangeState?.Invoke(false);
        }

        public async void StartBattle(IList<EnemyPack> enemies, IList<CompanionPack> companions)
        {
            _enemyPanel.DynamicInitialization(enemies.Take(5), _spawnDelay);
            _companyPanel.DynamicInitialization(companions.Take(5), _spawnDelay);

            if (enemies.Count > 5)
                _crowdPanel.Activate(enemies.Skip(5).ToList());

            await Show();
            _fightMechanism.StartBattle(_enemyPanel, _companyPanel);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}