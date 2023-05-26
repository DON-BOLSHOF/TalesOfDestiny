namespace Controllers.EventManagers
{
    public interface IEventManagerVisitor
    {
        public void Visit(EventManager eventManagerController);
    }
}