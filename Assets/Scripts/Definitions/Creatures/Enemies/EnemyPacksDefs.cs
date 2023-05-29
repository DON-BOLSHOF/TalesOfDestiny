using UnityEngine;

namespace Definitions.Creatures.Enemies
{
    [CreateAssetMenu(menuName = "Defs/EnemyPacksDefs", fileName = "EnemyPacksDefs")]
    public class EnemyPacksDefs : CardDefs<EnemyCard>
    {
        [SerializeField] private EnemiesType _enemiesType;

        public EnemiesType EnemiesType => _enemiesType;
    }
    
    public enum EnemiesType
    {
        Wolfs,
        Undeads,
        Rats,
        Orcs
    }
}