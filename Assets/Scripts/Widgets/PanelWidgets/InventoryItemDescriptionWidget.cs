using System;
using System.Collections;
using System.Collections.Generic;
using Definitions.Inventory;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils.Disposables;

namespace Widgets.PanelWidgets
{
    public class InventoryItemDescriptionWidget : PopUpHint
    {
        [SerializeField] private TextMeshPro _id;
        [SerializeField] private TextMeshPro _description;

        public Action OnThrownOut;
        public Action OnUsed;

        public override void Show()
        {
            gameObject.SetActive(true);
            StartRoutine(Show(_graphics), ref _graphicRoutine);
        }
        
        public void SetData(InventoryItem item)
        {
            _id.text = item.Id;
            _description.text = ConvertBuffsToString(item.Buffs);
        }
        
        public void OnThrowOutButtonClicked()
        {
            OnThrownOut?.Invoke();
        }

        public void OnUseButtonClicked()
        {
            OnUsed?.Invoke();
        }
        
        private string ConvertBuffsToString(List<BuffDef> itemBuffs)
        {
            var result = "Buff you ";

            foreach (var buff in itemBuffs)
            {
                switch (buff.BuffType)
                {
                    case BuffType.Army:
                    {
                        var armyBuff = buff.ArmyBuff;
                        result += armyBuff.ArmyBuffType switch
                        {
                            ArmyBuffType.Melee => " <b>melee</b> army:",
                            ArmyBuffType.Range => " <b>range</b> army:",
                            ArmyBuffType.Magic => " <b>mage</b> army:",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        if (armyBuff.AttackBuff != 0)
                        {
                            if (armyBuff.AttackBuff > 0) result += " <b>+</b>";
                            if (armyBuff.AttackBuff < 0) result += " <b>-</b>";
                            result += $" <b>{armyBuff.AttackBuff} attack</b>\n";
                        }

                        if (armyBuff.HealthBuff != 0)
                        {
                            if (armyBuff.HealthBuff > 0) result += " <b>+</b>";
                            if (armyBuff.HealthBuff < 0) result += " <b>-</b>";
                            result += $" <b>{armyBuff.HealthBuff} health</b>\n";
                        }

                        break;
                    }
                    case BuffType.Property:
                    {
                        var propertyBuff = buff.PropertyBuff;
                        if (propertyBuff.Coins != 0)
                        {
                            if (propertyBuff.Coins > 0) result += " <b>+</b>";
                            if (propertyBuff.Coins < 0) result += " <b>-</b>";
                            result += $" <b>{propertyBuff.Coins} gold</b>\n";
                        }
                        
                        if (propertyBuff.Prestige != 0)
                        {
                            if (propertyBuff.Prestige > 0) result += " <b>+</b>";
                            if (propertyBuff.Prestige < 0) result += " <b>-</b>";
                            result += $" <b>{propertyBuff.Prestige} prestige</b>\n";
                        }
                        
                        if (propertyBuff.Food != 0)
                        {
                            if (propertyBuff.Food > 0) result += " <b>+</b>";
                            if (propertyBuff.Food < 0) result += " <b>-</b>";
                            result += $" <b>{propertyBuff.Food} food</b>\n";
                        }
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        }

        public override void ForceExit()
        {
            _timer?.StopTimer();
            
            ChangeGraphicsAlphaTo(0);
            
            gameObject.SetActive(false);
        }

        protected override IEnumerator Disappear(Graphic[] graphics)
        {
            yield return base.Disappear(graphics);
            gameObject.SetActive(false);
        }

        public IDisposable SubscribeOnThrown(Action action)
        {
            OnThrownOut += action;
            return new ActionDisposable(() => OnThrownOut -= action);
        }
        
        public IDisposable SubscribeOnUsed(Action action)
        {
            OnUsed += action;
            return new ActionDisposable(() => OnUsed -= action);
        }
    }
}