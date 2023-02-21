using Panels;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class BattleBoardInstaller : MonoInstaller
    {
        [SerializeField] private BattleBoard _levelBoard;

        public override void InstallBindings()
        {
            Container.Bind<BattleBoard>().FromInstance(_levelBoard).AsSingle().NonLazy();
            Container.QueueForInject(_levelBoard);
        }
    }
}