using System;
using System.Collections.Generic;
using UnityEngine;

namespace Widgets
{
    public class PredefinedDataGroup<TPrefab, TItemData> : DataGroup<TPrefab, TItemData>
        where TPrefab : MonoBehaviour, IItemInstance<TItemData>
    {
        public PredefinedDataGroup(Transform container) : base(null, container)
        {
            var items = container.GetComponentsInChildren<TPrefab>();
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