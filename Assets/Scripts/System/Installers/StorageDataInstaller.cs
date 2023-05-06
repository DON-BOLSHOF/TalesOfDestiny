using Model;
using Model.Data.StorageData;
using Zenject;

namespace System.Installers
{
    public class StorageDataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StorageData>().AsSingle();
        }
    }
}