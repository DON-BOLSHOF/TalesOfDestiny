using UnityEngine;

namespace Cards.SituationCards.EndJourneyCards
{
    [CreateAssetMenu(menuName = "Cards/EndJourneyCard", fileName = "EndJourneyCard")]
    public class EndJourneyCard: SituationCard //Те же карты ситуации просто нужные для другого фасада
    {                                          //Думал надо сделать их только с кнопкой перезагрузки лвл-а и отмены,
                                               //но это же можно в геймплей впендюрить, так что просто доп картишки
                                               //которые обязаны будут на лвл-е появится
    }
}