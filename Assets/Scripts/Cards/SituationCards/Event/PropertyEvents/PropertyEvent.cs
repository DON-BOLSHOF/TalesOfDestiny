using System;
using Definitions.EventDefs;
using Model.Data.StorageData;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Interfaces;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [Serializable]
    public class PropertyEvent
    {
        [SerializeField] private PropertyMode _mode;

        [ShowIf(nameof(_mode), PropertyMode.Bound), SerializeField]
        private PropertyData _data;

        [ShowIf(nameof(_mode), PropertyMode.External), SerializeField]
        private PropertyEventDef _def;

        public PropertyData Data => _mode == PropertyMode.Bound ? _data : new CommonPropertyEvent(_data).Data;

        public void Accept(IPropertyVisitor visitor)
        {
            if (_mode == PropertyMode.External)
            {
                _def.Accept(visitor);
            }
            else
            {
                var propEvent = new CommonPropertyEvent(_data);
                propEvent.Accept(visitor);
            }
        }
        
        [Serializable]
        public enum PropertyMode
        {
            Bound,
            External
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

        public void Accept(IPropertyVisitor visitor)
        {
            visitor.VisitCommonPropEvent(this);
        }
    }
}