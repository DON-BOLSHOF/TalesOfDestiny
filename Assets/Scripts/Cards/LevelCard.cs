namespace Cards
{
    public abstract class LevelCard : Card
    {
        protected LevelCard(CardType type) : base(type)
        {
        }
    }

    public enum LevelCardType
    {
        Situation,
        Enemy,
        EndJourney
    }
}