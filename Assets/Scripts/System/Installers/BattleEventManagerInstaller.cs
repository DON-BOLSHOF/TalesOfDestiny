using Controllers;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class BattleEventManagerInstaller : MonoInstaller
    {
        [SerializeField] private BattleEventManager _manager;

        public override void InstallBindings()
        {
            Container.Bind<BattleEventManager>().FromInstance(_manager).AsSingle().NonLazy();
            Container.QueueForInject(_manager);
        }
    }
}