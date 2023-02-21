using System;
using Cards.SituationCards.Event;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Properties;
using UnityEngine;
using EventType = Cards.SituationCards.Event.EventType;

namespace Model.Data
{
    [Serializable]
    public class PlayerData : ICustomButtonVisitor
    {
        [SerializeField] private HeroPropertyData _heroData;

        public HeroPropertyData HeroData => _heroData;

        public void Visit(ButtonInteraction interaction)
        {
            if ((interaction.Type & EventType.PropertyVisitor) != EventType.PropertyVisitor) return;
            foreach (var propertyEvent in interaction.PropertyEvents)
            {
                propertyEvent.Accept(HeroData);
            }
        }
    }

    [Serializable]
    public class HeroPropertyData : IPropertyVisitor
    {
        public ObservableProperty<int> Food = new ObservableProperty<int>();
        public ObservableProperty<int> Coins = new ObservableProperty<int>();
        public ObservableProperty<int> Prestige = new ObservableProperty<int>();

        public HeroPropertyData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<HeroPropertyData>(json);
        }

        public void VisitCommonPropEvent(CommonPropEvent propEvent)
        {
            Food.Value = Food.Value - propEvent.Data.Food >=0? Food.Value - propEvent.Data.Food: 0;
            Prestige.Value = Prestige.Value - propEvent.Data.Prestige >=0? Prestige.Value - propEvent.Data.Prestige: 0;
            Coins.Value = Coins.Value - propEvent.Data.Coin >=0? Coins.Value - propEvent.Data.Coin: 0;
        }

        public void VisitPoisonEvent(PoisonEvent poisonEvent)
        {
            Food.Value /= poisonEvent.PoisonModifier;
        }
    }

    public interface IPropertyVisitor
    {
        void VisitCommonPropEvent(CommonPropEvent propEvent);
        void VisitPoisonEvent(PoisonEvent poisonEvent);
    }
}