using Controllers;

namespace Utils.Interfaces
{
    public interface IEventManagerVisitor
    {
        public void Visit(EventManager eventManagerController);
    }
}