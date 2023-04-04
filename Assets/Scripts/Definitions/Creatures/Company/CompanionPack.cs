using System;

namespace Definitions.Creatures.Company
{
    [Serializable]
    public class CompanionPack : CreaturePack
    {
        //[SerializeField] private CompanionCard _companion;

        private CompanionPack()
        { 
            //creatureCard = _companion;//Так инкапусулируем ввод только необходимых абстрактных типов
        }
        
        public CompanionPack(CompanionCard card, int count) : base(card, count)
        {
        }

        public CompanionPack(CompanionPack pack) : base(pack)
        {
        }
    }
}