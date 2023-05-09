namespace Utils.Interfaces
{
    public interface IGameStateVisitor
    {
        void VisitGameState(GameStateAnalyzer gameStateAnalyzer, Stage stage);
    }
}