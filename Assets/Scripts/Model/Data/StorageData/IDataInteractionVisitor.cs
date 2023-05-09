namespace Model.Data.StorageData
{
    public interface IDataInteractionVisitor
    {
        public void Visit(IDataInteraction interaction);
    }
}