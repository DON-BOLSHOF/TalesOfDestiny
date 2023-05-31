using LevelManipulation;
using Model;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class EventLevelBoardInstaller : MonoInstaller
    {
        [SerializeField] private EventLevelBoard eventLevelBoard;

        public override void InstallBindings()
        {
            Container.Bind<EventLevelBoard>().FromInstance(eventLevelBoard).AsSingle().NonLazy();
            Container.QueueForInject(eventLevelBoard);
        }
    }
}