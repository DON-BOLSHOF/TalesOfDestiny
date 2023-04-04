using Controllers;

namespace Utils.Interfaces
{
    public interface IBattleControllerVisitor
    {
        public void Visit(BattleController battleController);
    }
}