namespace Controllers.BattleControllers
{
    public interface IBattleControllerVisitor
    {
        public void Visit(BattleController battleController);
    }
}