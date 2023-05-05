using System;
using Definitions.EventDefs;
using Model.Data;
using UnityEngine;
using Utils.Interfaces;

namespace Cards.SituationCards.Event.PropertyEvents
{
    [Serializable]
    public class PropertyEvent
    {
        [SerializeField] private PropertyMode _mode;
        [SerializeField] private PropertyData _data;
        [SerializeField] private PropertyEventDef _def;

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

    [Serializable]
    public class PropertyData
    {
        [SerializeField] protected int _food;
        [SerializeField] protected int _coin;
        [SerializeField] protected int _prestige;

        public int Food => _food;
        public int Coin => _coin;
        public int Prestige => _prestige;
    }
}