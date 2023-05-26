using System;
using Definitions.EventDefs;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [Serializable]
    public class PropertyEvent
    {
        [SerializeField] private EventMode _mode;

        [ShowIf(nameof(_mode), EventMode.Bound), SerializeField]
        private PropertyData _data;

        [ShowIf(nameof(_mode), EventMode.External), SerializeField]
        private PropertyEventDef _def;

        public PropertyData Data => _mode == EventMode.Bound ? _data : new CommonPropertyEvent(_data).Data;

        public void Accept(IPropertyEventVisitor eventVisitor)
        {
            if (_mode == EventMode.External)
            {
                _def.Accept(eventVisitor);
            }
            else
            {
                var propEvent = new CommonPropertyEvent(_data);
                propEvent.Accept(eventVisitor);
            }
        }
    }

    public class CommonPropertyEvent
    {
        private PropertyData _data;

        public PropertyData Data => _data;

        public CommonPropertyEvent(PropertyData data)
        {
            _data = data;
        }

        public void Accept(IPropertyEventVisitor eventVisitor)
        {
            eventVisitor.VisitCommonPropEvent(this);
        }
    }
}