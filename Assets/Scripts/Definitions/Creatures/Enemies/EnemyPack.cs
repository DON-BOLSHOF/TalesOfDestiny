using System;
using UnityEngine;

namespace Definitions.Creatures.Enemies
{
    [Serializable]
    public class EnemyPack : CreaturePack
    {
        //[SerializeField] private EnemyCard _enemy;

        private EnemyPack()
        { 
            //creatureCard = _enemy;//Так инкапусулируем ввод только необходимых абстрактных типов
        }
        
        public EnemyPack(EnemyCard card, int count) : base(card, count)
        {
        }

        public EnemyPack(EnemyPack pack) : base(pack)
        {
        }
    }
}