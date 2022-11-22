using Definitions.LevelDefs;
using UnityEngine;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade: ScriptableObject
    {
        [SerializeField] private SituationCardDefs _situationCards;
        [SerializeField] private EnemyCardDefs _enemyCards;
        
        public SituationCardDefs SituationCards => _situationCards;
        public EnemyCardDefs EnemyCards => _enemyCards;
        
        private static DefsFacade _instance;

        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;
        
        private static DefsFacade LoadDefs()
        {
            return _instance = Resources.Load<DefsFacade>("Definitions/DefsFacade");
        }
    }
}