using System;
using UnityEngine;

namespace Cards.SituationCards.Event.ArmyEvents
{
    [Serializable]
    public class ArmyEvent
    {
        [SerializeField] private string _id;
        [SerializeField] private ArmyBuffType _armyBuffType;
        [SerializeField] private int _attackBuff;
        [SerializeField] private int _healthBuff;

        public string Id => _id;
        public ArmyBuffType ArmyBuffType => _armyBuffType;
        public int AttackBuff => _attackBuff;
        public int HealthBuff => _healthBuff;
    }

    public enum ArmyBuffType
    {
        Melee,
        Range,
        Magic
    }
}