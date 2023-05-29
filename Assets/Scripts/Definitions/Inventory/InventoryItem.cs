using System;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Inventory
{
    public class InventoryItem
    {
        public string Id { get; }
        public string Description { get; }
        public List<BuffDef> Buffs { get; }
        public Sprite Icon { get; }

        public InventoryItem(string id, string description, List<BuffDef> buffDefs, Sprite icon)
        {
            Id = id;
            Description = description;
            Buffs = buffDefs;
            Icon = icon;
        }
        
        public InventoryItemState ItemState => new Func<InventoryItemState>(() => //Для дальнейшего расширения
        {
            var hasActive = Buffs.FindIndex(item => item.BuffState == BuffState.Active) > -1;
            var hasPassive = Buffs.FindIndex(item => item.BuffState == BuffState.Passive) > -1;

            return hasActive switch
            {
                true when hasPassive => InventoryItemState.Both,
                true => InventoryItemState.Active,
                _ => InventoryItemState.Passive
            };
        })();
    }

    public enum InventoryItemState //Один будем перетаскивать, другой юзать, третий и так, и так можно будет
    {
        Passive,
        Active,
        Both
    }
}