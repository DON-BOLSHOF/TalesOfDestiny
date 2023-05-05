using Model;
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