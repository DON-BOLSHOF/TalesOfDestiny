using System;
using Definitions.Creatures;
using Model.Properties;

namespace Components
{
    public class HealthComponent
    {
        public ObservableProperty<int> Health { get; } = new ObservableProperty<int>();

        public void SetBaseValue(int health)
        {
            Health.SetBaseValue(health);
        }

        public void TakeDamage(int creatureAttack, AttackType creatureAttackType, ArmorType myArmorType)
        {
            Health.Value -= creatureAttackType switch
            {
                AttackType.Chopping => myArmorType switch
                {
                    ArmorType.Light => creatureAttack,
                    ArmorType.Medium => (int)(creatureAttack * 1.5f),
                    ArmorType.Heavy => creatureAttack,
                    _ => throw new ArgumentOutOfRangeException()
                },
                AttackType.Pricking => myArmorType switch
                {
                    ArmorType.Light => creatureAttack * 2,
                    ArmorType.Medium => (int)(creatureAttack * 0.75f),
                    ArmorType.Heavy => creatureAttack,
                    _ => throw new ArgumentOutOfRangeException()
                },
                AttackType.Magic => myArmorType switch
                {
                    ArmorType.Light => (int)(creatureAttack * 1.25f),
                    ArmorType.Medium => (int)(creatureAttack * 0.75f),
                    ArmorType.Heavy => creatureAttack * 2,
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException(nameof(creatureAttackType), creatureAttackType, null)
            };
        }
    }
}