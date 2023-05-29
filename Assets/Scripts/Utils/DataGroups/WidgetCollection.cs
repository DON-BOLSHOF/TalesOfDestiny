using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.DataGroups
{
    public class WidgetCollection<TWidget, TItemData> : IEnumerable<TWidget> where TWidget : WidgetInstance<TWidget, TItemData>
    {
        private readonly List<TWidget> _widgets;

        public WidgetCollection(Transform container)
        {
            var items = container.GetComponentsInChildren<TWidget>();
            _widgets = new List<TWidget>(items);
        }

        public void ReloadData(IList<TItemData> data)//Сделай при подбирании предметов выборку какие взять, какие выкинуть взамен при слишком большом кол-ве даты 
        {
            for(var i = 0; i<data.Count; i++) {
                _widgets[i].SetData(data[i]);
            }

            for (var i = data.Count; i < _widgets.Count; i++)
            {
                _widgets[i].Disable();
            }
        }

        public void SetAdditionallyData(IList<TItemData> eNewItems)
        {
            var index = 0;

            foreach (var widget in _widgets.Where(widget => widget.Stage == InstanceStage.Disabled && index<eNewItems.Count))
            {
                widget.SetData(eNewItems[index]);
                index++;
            }
        }

        public int FindIndex(TWidget widget)
        {
            return _widgets.FindIndex(prefab=> prefab.Equals(widget));
        }

        public int FindIndex(TItemData item)
        {
            return _widgets.FindIndex(prefab=> item.Equals(prefab.GetData()));
        }

        public void ChangeAtIndex(TItemData itemData, int index)
        {
            _widgets[index].SetData(itemData);
        }

        public void DisableAtIndex(int index)
        {
            if (_widgets.Count <= index) throw new ArgumentException("Index above instances!!!");
            
            _widgets[index].Disable();
        }

        public IEnumerator<TWidget> GetEnumerator()
        {
            return _widgets.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class WidgetInstance<TWidget, TItemData> : MonoBehaviour
    {
        public ReactiveEvent<TWidget> OnDisabled = new ReactiveEvent<TWidget>();
        public InstanceStage Stage { get; private set; }

        public virtual void SetData(TItemData pack)
        {
            Stage = InstanceStage.Activated;
        }
        
        public abstract TItemData GetData();

        public virtual void Disable()
        {
            Stage = InstanceStage.Disabled;
        }
    }

    public enum InstanceStage
    {
        Activated,
        Disabled
    }
}