using Model.Data;
using UnityEngine;
using Utils.Interfaces;

namespace Definitions.EventDefs
{
    public abstract class PropertyEventDef : ScriptableObject
    {
        public abstract void Accept(IPropertyVisitor visitor);
    }
}