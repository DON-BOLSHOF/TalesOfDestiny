using System.Collections.Generic;
using System.Linq;
using Definitions;
using Definitions.Creatures.Enemies;
using UnityEngine;

namespace Cards
{
    public static class EnemyCardFactory
    {
        public static List<EnemyPack> GeneratePacks(EnemiesType enemiesType, int maximumPacks, int turnThreshold, IList<EnemyPack> obligatoryEnemies = null)
        {
            var resultPacks = new List<EnemyPack>();

            if (obligatoryEnemies != null) resultPacks.AddRange(obligatoryEnemies.Select(pack => new EnemyPack(pack)));
            var enemyCount = resultPacks.Count;
       
            var enemies = DefsFacade.I.EnemyDefs.GetEnemyPack(enemiesType).GetAllCardsDefs();
            foreach (var enemy in enemies.Where(t => enemyCount<=maximumPacks))
            {
                EnemyPack creaturePack = null;
                if (!GenerateRandomPack(enemy, turnThreshold, ref creaturePack)) continue;

                var existedPack = resultPacks.Find(card => card.CreatureCard.Equals(enemy));
                if (existedPack != null)
                {
                    existedPack.ModifyCount(creaturePack.Count);
                }
                else
                {
                    resultPacks.Add(creaturePack);
                }

                enemyCount++;
            }

            return resultPacks;
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