using Definitions.LevelDefs;
using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade: ScriptableObject
    {
        [SerializeField] private SituationCardDefs _situationCards;
        [SerializeField] private HeroCardDefs _heroCards;
        [SerializeField] private EnemyCardDefs _enemyCards;
        [SerializeField] private EndJourneyCardDefs _endJourneyCards;
        
        public SituationCardDefs SituationCards => _situationCards;
        public EnemyCardDefs EnemyCards => _enemyCards;

        public EndJourneyCardDefs EndJourneyCards => _endJourneyCards;

        public HeroCardDefs HeroCardDefs => _heroCards;
        
        private static DefsFacade _instance;

        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;
        
        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("Definitions/DefsFacade");
        }
    }
}