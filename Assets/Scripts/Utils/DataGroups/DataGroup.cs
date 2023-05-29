using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils.DataGroups
{
    public class DataGroup<TPrefab, TItemData> where TPrefab : MonoBehaviour, IItemInstance<TItemData>
    {
        protected Transform _container;
        protected TPrefab _prefab;

        protected List<TPrefab> CreatedInstances = new List<TPrefab>();

        public DataGroup(TPrefab prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;
        }

        public virtual void SetData(IList<TItemData> list)
        {
            Instantiate(list);

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

        protected virtual void Instantiate(IList<TItemData> list)
        {
            for (var i = CreatedInstances.Count; i < list.Count; i++)
            {
                var instantiate = Object.Instantiate(_prefab, _container);
                CreatedInstances.Add(instantiate);
            }
        }
    }

    public interface IItemInstance<TItemData>
    {
        void SetData(TItemData pack);
    }
}