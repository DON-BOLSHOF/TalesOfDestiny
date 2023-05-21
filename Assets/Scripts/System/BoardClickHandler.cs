using Cards;
using Cards.EndJourneyCards;
using Cards.SituationCards;
using Controllers;
using Zenject;

namespace System
{
    public class BoardClickHandler
    {
        [Inject] protected BattleEventManager _battleEventManager;
        [Inject] protected EventManager _eventManager;

        public void Click(LevelCard levelCard)
        {
            switch (levelCard)
            {
                case BattleCard _:
                    _battleEventManager.ShowEventContainer(levelCard);//Замени на визитор
                    break;
                case EndJourneyCard _:
                case SituationCard _:
                    _eventManager.ShowEventContainer(levelCard);
                    break;
            }
        }
    }
}