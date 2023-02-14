namespace Cards
{
    public abstract class LevelCard : Card
    {
        private LevelCardType _levelCardType;

        public LevelCardType LevelCardType => _levelCardType;
        
        protected LevelCard(CardType type, LevelCardType levelLevelCardType) : base(type)
        {
            _levelCardType = levelLevelCardType;
        }
    }

    public enum LevelCardType
    {
        HeroPosition,
        Situation,
        Battle,
        EndJourney
    }
}