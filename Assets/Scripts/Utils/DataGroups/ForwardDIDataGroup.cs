using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Utils.DataGroups
{
    public class ForwardDiDataGroup<TPrefab, TItemData> : DataGroup<TPrefab, TItemData>
        where TPrefab : MonoBehaviour, IItemInstance<TItemData>
    {
        private DiContainer _diContainer;

        public ForwardDiDataGroup(TPrefab prefab, Transform container, DiContainer diContainer) : base(prefab,
            container)
        {
            _diContainer = diContainer;
        }

        protected override void Instantiate(IList<TItemData> list)
        {
            for (int i = CreatedInstances.Count; i < list.Count; i++)
            {
                var instantiate = _diContainer.InstantiatePrefab(_prefab, _container);

                CreatedInstances.Add(instantiate.GetComponent<TPrefab>());
            }
        }
    }
}