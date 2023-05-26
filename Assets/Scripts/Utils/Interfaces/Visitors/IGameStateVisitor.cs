namespace Utils.Interfaces.Visitors
{
    public interface IGameStateVisitor
    {
        void VisitGameState(GameStateAnalyzer gameStateAnalyzer, Stage stage);
    }
}