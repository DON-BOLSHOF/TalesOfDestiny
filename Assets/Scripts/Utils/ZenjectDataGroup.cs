using System.Collections.Generic;
using UnityEngine;
using Utils.Interfaces;

namespace Utils
{
    public class ZenjectDataGroup<TPrefab, TItemData> : DataGroup<TPrefab, TItemData>
        where TPrefab : ZenjectDynamicObject<TPrefab>,
        IItemInstance<TItemData> //Перспектива рабочая, но пока перегорел... Доработать в будущем, кнопки заменить!
    {
        private IZenjectSpawner<TPrefab> _spawner;

        public ZenjectDataGroup(TPrefab prefab, Transform container, IZenjectSpawner<TPrefab> spawner) : base(prefab,
            container)
        {
            _spawner = spawner;
        }

        protected override void Instantiate(IList<TItemData> list)
        {
            for (int i = CreatedInstances.Count; i < list.Count; i++)
            {
                var instantiate = _spawner.Spawn();
                instantiate.transform.parent = _container.transform;

                CreatedInstances.Add(instantiate);
            }
        }
    }
}