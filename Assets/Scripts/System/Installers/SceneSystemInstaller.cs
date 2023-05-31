using Model;
using UI;
using Zenject;

namespace System.Installers
{
    public class SceneSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BoardClickHandler>().AsSingle().NonLazy();
            Container.Bind<NotificationHandler>().AsSingle().NonLazy();
            Container.Bind<GlobalHeroMover>().AsSingle().NonLazy();
            Container.Bind<LocalHeroMover>().AsSingle().NonLazy();
        }
    }
}