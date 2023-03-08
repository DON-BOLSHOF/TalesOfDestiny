using Panels;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class BattleBoardInstaller : MonoInstaller
    {
        [SerializeField] private BattleBoard levelBoard;

        public override void InstallBindings()
        {
            Container.Bind<BattleBoard>().FromInstance(levelBoard).AsSingle().NonLazy();
            Container.QueueForInject(levelBoard);
        }
    }
}