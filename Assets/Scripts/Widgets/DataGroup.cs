using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class DataGroup<TPrefab, TItemData> where TPrefab : MonoBehaviour, IItemInstance<TItemData>
    {
        private Transform _container;
        private TPrefab _prefab;

        protected List<TPrefab> CreatedInstances = new List<TPrefab>();

        public DataGroup(TPrefab prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TItemData> list)
        {
            for (int i = CreatedInstances.Count; i < list.Count; i++)
            {
                var instantiate =  Object.Instantiate(_prefab, _container);
                CreatedInstances.Add(instantiate);
            }

            for (int i = 0; i < list.Count; i++)
            {
                CreatedInstances[i].gameObject.SetActive(true);
                CreatedInstances[i].SetData(list[i]);
            }

            for (int i = list.Count; i < CreatedInstances.Count; i++)
            {
                CreatedInstances[i].gameObject.SetActive(false);
            }
        }
    }

    public interface IItemInstance<TItemData>
    {
        void SetData(TItemData data);
    }
}