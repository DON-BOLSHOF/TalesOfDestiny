using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Enemies
{
    [CreateAssetMenu(menuName = "Defs/EnemyDefs", fileName = "EnemyDefs")]
    public class EnemyDefs : ScriptableObject
    {
        [SerializeField] private List<EnemyCard> _enemies;

        public List<EnemyCard> Enemies => _enemies;
        public int EnemiesCount => _enemies.Count;

        public EnemyCard Get(string id)
        {
            if (id == null)
                return default;

            foreach (var itemDef in _enemies)
            {
                if (itemDef.Id == id)
                {
                    return itemDef;
                }
            }

            return default;
        }

        public EnemyCard Get(int id)
        {
            return _enemies[id];
        }
    }
}