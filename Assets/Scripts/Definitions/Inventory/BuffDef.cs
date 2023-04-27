using System;
using NaughtyAttributes;
using UnityEngine;

namespace Definitions.Inventory
{
    [Serializable]
    public class BuffDef
    {
        [SerializeField] private BuffStateType _buffStateType;
        [SerializeField] private BuffType _buffType;

        [ShowIf(nameof(_buffType), BuffType.Army), SerializeField]
        private ArmyBuff _armyBuff;

        [ShowIf(nameof(_buffType), BuffType.Property), SerializeField]
        private PropertyBuff _propertyBuff;

        public BuffStateType BuffStateType => _buffStateType;
        public BuffType BuffType => _buffType;
        public ArmyBuff ArmyBuff => _armyBuff;
        public PropertyBuff PropertyBuff => _propertyBuff;
    }

    [Serializable]
    public class ArmyBuff
    {
        [SerializeField] private ArmyBuffType _armyBuffType;
        [SerializeField] private int _attackBuff;
        [SerializeField] private int _healthBuff;

        public ArmyBuffType ArmyBuffType => _armyBuffType;
        public int AttackBuff => _attackBuff;
        public int HealthBuff => _healthBuff;
    }

    [Serializable]
    public class PropertyBuff
    {
        [SerializeField] private int _coins;
        [SerializeField] private int _prestige;
        [SerializeField] private int _food;

        public int Coins => _coins;
        public int Prestige => _prestige;
        public int Food => _food;
    }

    public enum
        BuffStateType // Используется внутри инвенторя(условные кнопки пополняющие еду), Или чисто перетаскиваемые шмотки в армию/куда-нибудь еще мб понадобится 
    {
        Active,
        Passive
    }

    public enum BuffType
    {
        Army,
        Property
    }

    public enum ArmyBuffType
    {
        Melee,
        Range,
        Magic
    }
}