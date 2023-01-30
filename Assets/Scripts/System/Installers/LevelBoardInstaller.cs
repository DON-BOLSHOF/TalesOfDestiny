using LevelManipulation;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class LevelBoardInstaller : MonoInstaller
    {
        [SerializeField] private LevelBoard _levelBoard;

        public override void InstallBindings()
        {
            Container.Bind<LevelBoard>().FromInstance(_levelBoard).AsSingle().NonLazy();
            Container.QueueForInject(_levelBoard);
        }
    }
}