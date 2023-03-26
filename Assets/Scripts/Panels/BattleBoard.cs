﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Definitions.Creatures.Enemies;
using Model;
using UnityEngine;
using Utils.Disposables;

namespace Panels
{
    public class BattleBoard : MonoBehaviour
    {
        [SerializeField] private CreaturePanel _enemyPanel;
        [SerializeField] private CreaturePanel _heroAllyPanel;
        [SerializeField] private CrowdPanel _crowdPanel;

        [SerializeField] private FightMechanism _fightMechanism;
        
        private DisposeHolder _trash = new DisposeHolder();

        public event Action<bool> OnChangeState;

        private void Awake()
        {
            _trash.Retain(new Func<IDisposable>(() =>
            {
                _heroAllyPanel.OnExit += Exit;
                return new ActionDisposable(() => _heroAllyPanel.OnExit -= Exit);
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
            await _heroAllyPanel.Show();
        }

        private void Exit()
        {
            _fightMechanism.StopBattle();
            OnChangeState?.Invoke(false);

            _enemyPanel.Deactivate();
            _heroAllyPanel.Deactivate();
        }

        public async void StartBattle(IList<EnemyPack> enemies)
        {
            _enemyPanel.DynamicInitialization(enemies.Take(5));
            _heroAllyPanel.DynamicInitialization(enemies.Take(5));

            if (enemies.Count > 5)
                _crowdPanel.Activate(enemies.Skip(5).ToList());

            await Show();
            _fightMechanism.StartBattle(_enemyPanel, _heroAllyPanel);
        }
        
        public void TakeAdditivelyPanelSubscribes(AbstractPanelUtil panel)
        {
            OnChangeState += panel.OnChangeState;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}