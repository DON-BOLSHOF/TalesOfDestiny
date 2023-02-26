using Controllers;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class BattleControllerInstaller : MonoInstaller
    {
        [SerializeField] private BattleController _controller;

        public override void InstallBindings()
        {
            Container.Bind<BattleController>().FromInstance(_controller).AsSingle().NonLazy();
            Container.QueueForInject(_controller);
        }
    }
}