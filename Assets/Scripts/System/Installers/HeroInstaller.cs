using Components.ZenjectSpawnComponents;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class HeroInstaller : MonoInstaller
    {
        [SerializeField] private HeroBehaviour _hero;

        public override void InstallBindings()
        {
            Container.Bind<HeroSpawnerComponent>().AsSingle();
            Container.BindFactory<HeroBehaviour, HeroBehaviour.Factory>().FromComponentInNewPrefab(_hero);
        }
    }
}