using Zenject;

namespace System.Installers
{
    public class SceneSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BoardClickHandler>().AsSingle().NonLazy();
        }
    }
}