using System.Collections.Generic;
using System.Linq;
using Definitions;
using Definitions.Enemies;
using UnityEngine;

namespace Cards
{
    public static class EnemyCardFactory
    {
        public static List<EnemyPack> GeneratePacks(int turnThreshold, IList<EnemyPack> obligatoryEnemies = null)
        {
            var result = new List<EnemyPack>();

            if (obligatoryEnemies != null) result.AddRange(obligatoryEnemies.Select(pack => new EnemyPack(pack)));
       
            var enemies = DefsFacade.I.EnemyDefs.Enemies;
            foreach (var enemy in enemies)
            {
                EnemyPack enemyPack = null;
                if (!GenerateRandomPack(enemy, turnThreshold, ref enemyPack)) continue;

                var existedPack = result.Find(card => card.EnemyCard.Equals(enemy));
                if (existedPack != null)
                {
                    existedPack.ModifyCount(enemyPack.Count);
                }
                else
                {
                    result.Add(enemyPack);
                }
            }

            foreach (var pack in result)
            {
                Debug.Log(pack);
            }

            return result;
        }

        private static bool GenerateRandomPack(EnemyCard enemyCard, int turnThreshold, ref EnemyPack pack)
        {
            if (turnThreshold < enemyCard.TurnThreshold) return false;

            var count = Random.Range(0, turnThreshold - enemyCard.TurnThreshold);
            Debug.Log(count);
            if (count <= 0) return false;
            pack = new EnemyPack(enemyCard, count);
            return true;
        }
    }
}