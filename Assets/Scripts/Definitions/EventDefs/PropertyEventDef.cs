using Model.Data;
using UnityEngine;

namespace Definitions.EventDefs
{
    public abstract class PropertyEventDef : ScriptableObject
    {
        public abstract void Accept(IPropertyVisitor visitor);
    }
}