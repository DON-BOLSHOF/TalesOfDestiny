using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class WidgetCollection<TWidget, TItemData> where TWidget : MonoBehaviour, IWidgetInstance<TItemData>
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

        public int FindIndex(TItemData itemData)
        {
            return _widgets.FindIndex(prefab=> prefab.GetData().Equals(itemData));
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
    }

    public interface IWidgetInstance<ItemData> : IItemInstance<ItemData>
    {
        InstanceStage InstanceStage { get; }
        ItemData GetData();
        void Disable();
    }

    public enum InstanceStage
    {
        Enabled,
        Disabled
    }
}