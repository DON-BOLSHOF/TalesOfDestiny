using Definitions.Enemies;
using Definitions.LevelDefs;
using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private SituationCardDefs _situationCards;
        [SerializeField] private HeroCardDefs _heroCards;
        [SerializeField] private BattleCardDefs battleCards;
        [SerializeField] private EndJourneyCardDefs _endJourneyCards;
        [SerializeField] private EnemyDefs _enemyDefs;

        public SituationCardDefs SituationCards => _situationCards;
        public BattleCardDefs BattleCards => battleCards;
        public EndJourneyCardDefs EndJourneyCards => _endJourneyCards;
        public HeroCardDefs HeroCardDefs => _heroCards;
        public EnemyDefs EnemyDefs => _enemyDefs;

        private static DefsFacade _instance;

        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("Definitions/DefsFacade");
        }
    }
}