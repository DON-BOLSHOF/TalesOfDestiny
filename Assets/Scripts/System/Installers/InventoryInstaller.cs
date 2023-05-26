using Controllers;
using Controllers.Inventories;
using UnityEngine;
using Zenject;

namespace System.Installers
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private Inventory _inventory;
        
        public override void InstallBindings()
        {
            Container.Bind<Inventory>().FromInstance(_inventory).AsSingle().NonLazy();
            Container.QueueForInject(_inventory);
        }
    }
}