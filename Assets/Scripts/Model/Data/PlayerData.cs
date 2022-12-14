using System;
using Cards.SituationCards.Event.PropertyEvents;
using Model.Properties;
using UnityEngine;

namespace Model.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private HeroPropertyData _heroData;

        public HeroPropertyData HeroData => _heroData;
    }

    [Serializable]
    public class HeroPropertyData : IPropertyVisitor
    {
        public ObservableProperty<float> Food = new ObservableProperty<float>();
        public ObservableProperty<float> Coins = new ObservableProperty<float>();
        public ObservableProperty<float> Prestige = new ObservableProperty<float>();

        public HeroPropertyData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<HeroPropertyData>(json);
        }

        public void VisitCommonPropEvent(CommonPropEvent propEvent)
        {
            Food.Value -= propEvent.Data.Food;
            Prestige.Value -= propEvent.Data.Prestige;
            Coins.Value -= propEvent.Data.Coin;
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