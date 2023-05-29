using System.Linq;
using UnityEngine;

namespace Definitions.Creatures.Enemies
{
    [CreateAssetMenu(menuName = "Defs/EnemyDefs", fileName = "EnemyDefs")]
    public class EnemyDefs : ScriptableObject
    {
        [SerializeField] private EnemyPacksDefs[] _enemyPacksDefs;

        public EnemyPacksDefs[] EnemyPacksDefs => _enemyPacksDefs;

        public EnemyPacksDefs GetEnemyPack(EnemiesType enemiesType)
        {
            var result = _enemyPacksDefs.First(t => t.EnemiesType == enemiesType);
            return result;
        }
    }
}