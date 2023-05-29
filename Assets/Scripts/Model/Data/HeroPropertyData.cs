using System;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Properties;
using Model.Tributes;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class HeroPropertyData : IPropertyEventVisitor, ITributeVisitor
    {
        public ObservableProperty<int> Food = new ObservableProperty<int>();
        public ObservableProperty<int> Coins = new ObservableProperty<int>();
        public ObservableProperty<int> Prestige = new ObservableProperty<int>();

        public HeroPropertyData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<HeroPropertyData>(json);
        }

        public void VisitCommonPropEvent(CommonPropertyEvent propertyEvent)//При разрастании ивентов - это все станет излишними подробностями
        {
            Food.Value = Food.Value + propertyEvent.Data.Food >=0? Food.Value+propertyEvent.Data.Food: 0;
            Prestige.Value = Prestige.Value + propertyEvent.Data.Prestige >=0? Prestige.Value + propertyEvent.Data.Prestige: 0;
            Coins.Value = Coins.Value + propertyEvent.Data.Coins >=0? Coins.Value + propertyEvent.Data.Coins: 0;
        }

        public void VisitPoisonEvent(PoisonEvent poisonEvent)
        {
            Food.Value /= poisonEvent.PoisonModifier;
        }

        public void VisitBurningCampEvent(BurningCampEvent burningCampEvent)
        {
            Coins.Value = (int)(Coins.Value *  (1-burningCampEvent.DamagePerCent));
        }

        public void Visit(ITribute tribute)
        {
            Food.Value = Food.Value - tribute.TributePropertyData.Food >=0? Food.Value-tribute.TributePropertyData.Food: 0;
            Coins.Value = Coins.Value - tribute.TributePropertyData.Coins >=0? Coins.Value-tribute.TributePropertyData.Coins: 0;
            Prestige.Value = Prestige.Value - tribute.TributePropertyData.Prestige >=0? Prestige.Value-tribute.TributePropertyData.Prestige: 0;
        }
    }
}