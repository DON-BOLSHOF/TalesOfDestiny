namespace Utils.Interfaces
{
    public interface IZenjectSpawner<T>: ISpawner<T> where T: ZenjectDynamicObject<T>
    {
        ZenjectDynamicObject<T>.Factory _factory { get; set; }
    }
}