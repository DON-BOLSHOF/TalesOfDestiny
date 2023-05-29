using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.DataGroups
{
    public class PredefinedDataGroup<TPrefab, TItemData> : DataGroup<TPrefab, TItemData>
        where TPrefab : MonoBehaviour, IItemInstance<TItemData>
    {
        public PredefinedDataGroup(Transform container, bool findInActive = false) : base(null, container)
        {
            var items = container.GetComponentsInChildren<TPrefab>(findInActive);
            CreatedInstances.AddRange(items);
        }

        public override void SetData(IList<TItemData> list)
        {
            if (list.Count > CreatedInstances.Count)
                throw new ArgumentException("Too many instances");

            base.SetData(list);
        }
    }
}