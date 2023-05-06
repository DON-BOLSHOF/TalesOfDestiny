using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Disposables;

namespace Utils
{
    public class WidgetCollection<TWidget, TItemData> where TWidget : WidgetInstance<TWidget, TItemData>
    {
        private readonly List<TWidget> _widgets;

        public WidgetCollection(Transform container)
        {
            var items = container.GetComponentsInChildren<TWidget>();
            _widgets = new List<TWidget>(items);
        }

        public void ReloadData(IList<TItemData> data)//Сделай при подбирании предметов выборку какие взять, какие выкинуть взамен при слишком большом кол-ве даты 
        {
            for(int i = 0; i<data.Count; i++) {
                _widgets[i].SetData(data[i]);
            }

            for (int i = data.Count; i < _widgets.Count; i++)
            {
                _widgets[i].Disable();
            }
        }

        public int FindIndex(TWidget widget)
        {
            return _widgets.FindIndex(prefab=> prefab.Equals(widget));
        }
        
        public int FindIndex(TItemData item)
        {
            return _widgets.FindIndex(prefab=> prefab.GetData().Equals(item));
        }

        public void ChangeAtIndex(TItemData itemData, int index)
        {
            _widgets[index].SetData(itemData);
        }

        public void SetAdditionallyData(IList<TItemData> list)
        {
            var listIndex = 0;

            foreach (var t in _widgets.Where(t => t.InstanceStage == InstanceStage.Disabled))
            {
                t.SetData(list[listIndex]);
                listIndex++;
            }
        }

        public void DisableAtIndex(int index)
        {
            if (_widgets.Count <= index) throw new ArgumentException("Index above instances!!!");
            
            _widgets[index].Disable();
        }

        public List<IDisposable> SubscribeToAll(Action<TWidget> call)
        {
            var result = new List<IDisposable>();
            foreach (var widget in _widgets)
            {
                widget.OnDisabled += call;
                result.Add(new ActionDisposable(() => widget.OnDisabled-=call));
            }

            return result;
        }
    }

    public abstract class WidgetInstance<TWidget, TItemData> : MonoBehaviour
    {
        public Action<TWidget> OnDisabled;
        public abstract void SetData(TItemData pack);
        public abstract InstanceStage InstanceStage { get; set; }
        public abstract TItemData GetData();
        public abstract void Disable();
    }

    public enum InstanceStage
    {
        Enabled,
        Disabled
    }
}