﻿using System;
using Definitions;
using Random = UnityEngine.Random;

namespace Cards
{
    public static class LevelCardFactory
    {
        public static LevelCard GetLevelCard(LevelCardType type, string id)
        {
            switch (type)
            {
                case LevelCardType.Situation:
                    return DefsFacade.I.SituationCards.Get(id);
                case LevelCardType.Enemy:
                    return DefsFacade.I.EnemyCards.Get(id);
                case LevelCardType.EndJourney:
                    return DefsFacade.I.EndJourneyCards.Get(id);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static LevelCard GetLevelCardRandomly()
        {
            var type =(LevelCardType)Random.Range((int)LevelCardType.Situation, (int)LevelCardType.Situation);

            var card = GetFromListRandomly(type);

            return card;
        }

        public static LevelCard GetFromListRandomly(LevelCardType type) //Честно хз как это еще назвать
        {
            LevelCard card = default;
            
            switch (type)
            {
                case LevelCardType.Situation:
                {
                    var length = DefsFacade.I.SituationCards.CardsCount;
                    var res = Random.Range(0, length);
                    card = DefsFacade.I.SituationCards.Get(res);
                    break;
                }
                case LevelCardType.Enemy:
                {
                    var length = DefsFacade.I.EnemyCards.CardsCount;
                    var res = Random.Range(0, length);
                    card = DefsFacade.I.EnemyCards.Get(res);
                    break;
                }
                case LevelCardType.EndJourney:
                {
                    var length = DefsFacade.I.EndJourneyCards.CardsCount;
                    var res = Random.Range(0, length);
                    card = DefsFacade.I.EndJourneyCards.Get(res);
                    break;
                }
                case LevelCardType.HeroPosition:
                {
                    var length = DefsFacade.I.EndJourneyCards.CardsCount;
                    var res = Random.Range(0, length);
                    card = DefsFacade.I.HeroCardDefs.Get(res);
                    break;
                }
            }

            return card;
        }
    }
}