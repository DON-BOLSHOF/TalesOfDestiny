using Model.Data;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class GameSessionInstaller : MonoInstaller
    {
        [SerializeField] private GameSession _gameSession;

        public override void InstallBindings()
        {
            var gameSessionInstance = Container.InstantiatePrefabForComponent<GameSession>(_gameSession);
            
            Container.Bind<GameSession>().FromInstance(gameSessionInstance).AsSingle().NonLazy();
            Container.QueueForInject(gameSessionInstance);
            Container.BindInterfacesTo<HeroInventoryData>().FromInstance(gameSessionInstance.Data.InventoryData).AsSingle();
        }
    }
}