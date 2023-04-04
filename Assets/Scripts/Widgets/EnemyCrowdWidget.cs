﻿using System.Collections.Generic;
using Definitions.Creatures;
using Definitions.Creatures.Enemies;
using UnityEngine;
using Utils;
using View.CreaturesCardView.EnemyCardView;

namespace Widgets
{
    public class EnemyCrowdWidget : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform _enemyCardContainer;

        private PredefinedDataGroup<EnemyCardViewWidget, EnemyPack> _dataGroup;

        private static readonly int ShowKey = Animator.StringToHash("Show");
        private static readonly int VelocityKey = Animator.StringToHash("Velocity");
        private static readonly int PrepareToBattleKey = Animator.StringToHash("PrepareToBattle");

        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }

        private void OnMouseEnter()
        {
            Show(true);
        }

        private void OnMouseExit()
        {
            Show(false);
        }


        private void Show(bool value)
        {
            _animator.SetFloat(VelocityKey, value ? 1f : -1f);
            _animator.SetBool(ShowKey, value);
        }

        public void SetEnemyData(List<EnemyPack> packs)
        {
            _dataGroup ??= new PredefinedDataGroup<EnemyCardViewWidget, EnemyPack>(_enemyCardContainer, true);
            _dataGroup.SetData(packs);
        }

        public void PrepareToBattle()//На будущее, если рассинхрон между аниматорами интерфейса будет: Делай через твины... 
        {
            _animator.SetTrigger(PrepareToBattleKey);
        }
    }
}