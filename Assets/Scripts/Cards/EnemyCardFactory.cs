using System.Collections.Generic;
using System.Linq;
using Definitions;
using Definitions.Creatures;
using Definitions.Creatures.Enemies;
using UnityEngine;

namespace Cards
{
    public static class EnemyCardFactory
    {
        public static List<EnemyPack> GeneratePacks(int turnThreshold, IList<EnemyPack> obligatoryEnemies = null)
        {
            var result = new List<EnemyPack>();

            if (obligatoryEnemies != null) result.AddRange(obligatoryEnemies.Select(pack => new EnemyPack(pack)));
       
            var enemies = DefsFacade.I.EnemyDefs.GetAllCardsDefs();
            foreach (var enemy in enemies)
            {
                EnemyPack creaturePack = null;
                if (!GenerateRandomPack(enemy, turnThreshold, ref creaturePack)) continue;

                var existedPack = result.Find(card => card.CreatureCard.Equals(enemy));
                if (existedPack != null)
                {
                    existedPack.ModifyCount(creaturePack.Count);
                }
                else
                {
                    result.Add(creaturePack);
                }
            }

            return result;
        }

        private static bool GenerateRandomPack(EnemyCard enemyCard, int turnThreshold, ref EnemyPack pack)
        {
            if (turnThreshold < enemyCard.TurnThreshold) return false;

            var count = Random.Range(0, turnThreshold - enemyCard.TurnThreshold);
            if (count <= 0) return false;
            pack = new EnemyPack(enemyCard, count);
            return true;
        }
    }
}