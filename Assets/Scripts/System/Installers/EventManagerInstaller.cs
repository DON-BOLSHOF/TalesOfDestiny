using Controllers;
using Controllers.EventManagers;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class EventManagerInstaller: MonoInstaller
    {
        [SerializeField] private EventManager _manager;

        public override void InstallBindings()
        {
            Container.Bind<EventManager>().FromInstance(_manager).AsSingle().NonLazy();
            Container.QueueForInject(_manager);
        }
    }
}