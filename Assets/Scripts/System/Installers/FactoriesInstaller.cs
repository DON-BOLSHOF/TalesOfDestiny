using Cards;
using Zenject;

namespace System.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemWidgetFactory>().AsSingle().NonLazy();
        }
    }
}