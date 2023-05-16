using System;
using System.Collections.Generic;
using Cards.SituationCards.Event.ArmyEvents;
using Definitions.Inventory;
using Model.Data.StorageData;

namespace Utils
{
    public static class ConverterUtils
    {
         public static string ConvertBuffsToString(List<BuffDef> itemBuffs)
        {
            var result = "Buff you ";

            foreach (var buff in itemBuffs)
            {
                if ((buff.DataType & DataInteractionType.ArmyVisitor) == DataInteractionType.ArmyVisitor)
                {
                    foreach (var army in buff.ArmyEvents)
                    {
                        var armyBuff = army.ArmyBuffType;
                        result += armyBuff switch
                        {
                            ArmyBuffType.Melee => " <b>melee</b> army:",
                            ArmyBuffType.Range => " <b>range</b> army:",
                            ArmyBuffType.Magic => " <b>mage</b> army:",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        if (army.AttackBuff != 0)
                        {
                            if (army.AttackBuff > 0) result += " <b>+</b>";
                            if (army.AttackBuff < 0) result += " <b>-</b>";
                            result += $" <b>{army.AttackBuff} attack</b>\n";
                        }

                        if (army.HealthBuff != 0)
                        {
                            if (army.HealthBuff > 0) result += " <b>+</b>";
                            if (army.HealthBuff < 0) result += " <b>-</b>";
                            result += $" <b>{army.HealthBuff} health</b>\n";
                        }
                    }
                }

                if ((buff.DataType & DataInteractionType.PropertyVisitor) == DataInteractionType.PropertyVisitor)
                {
                    {
                        foreach (var propertyBuff in buff.PropertyEvents)
                        {
                            if (propertyBuff.Data.Coins != 0)
                            {
                                if (propertyBuff.Data.Coins > 0) result += " <b>+</b>";
                                if (propertyBuff.Data.Coins < 0) result += " <b>-</b>";
                                result += $" <b>{propertyBuff.Data.Coins} gold</b>\n";
                            }

                            if (propertyBuff.Data.Prestige != 0)
                            {
                                if (propertyBuff.Data.Prestige > 0) result += " <b>+</b>";
                                if (propertyBuff.Data.Prestige < 0) result += " <b>-</b>";
                                result += $" <b>{propertyBuff.Data.Prestige} prestige</b>\n";
                            }

                            if (propertyBuff.Data.Food != 0)
                            {
                                if (propertyBuff.Data.Food > 0) result += " <b>+</b>";
                                if (propertyBuff.Data.Food < 0) result += " <b>-</b>";
                                result += $" <b>{propertyBuff.Data.Food} food</b>\n";
                            }
                        }
                    }
                }
            }

            return result;
        }

    }
}