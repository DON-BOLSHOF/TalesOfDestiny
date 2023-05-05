using System;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Inventory
{
    [Serializable]
    public class InventoryItem
    {
        [SerializeField] private string _id;
        [SerializeField] private List<BuffDef> _buffs;
        [SerializeField] private Sprite _icon;

        public string Id => _id;
        public List<BuffDef> Buffs => _buffs;
        public Sprite Icon => _icon;

        public InventoryItemState ItemState => new Func<InventoryItemState>(() =>
        {
            var hasActive = _buffs.FindIndex(item => item.BuffState == BuffState.Active) > -1;
            var hasPassive = _buffs.FindIndex(item => item.BuffState == BuffState.Passive) > -1;

            return hasActive switch
            {
                true when hasPassive => InventoryItemState.Both,
                true => InventoryItemState.Active,
                _ => InventoryItemState.Passive
            };
        })();
    }

    public enum InventoryItemState//Один будем перетаскивать, другой юзать, третий и так, и так можно будет
    {
        Passive,
        Active,
        Both
    }
}