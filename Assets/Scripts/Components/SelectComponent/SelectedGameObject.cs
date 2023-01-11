using System;
using UnityEngine;

namespace Components.SelectComponent
{
    public class SelectedGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject _selection;

        public Action<SelectedGameObject> ISelected;
    
        private void OnMouseEnter()
        {
            ISelected?.Invoke(this);
        }

        public void ActivateSelection(bool activation)
        {
            _selection.SetActive(activation);
        }
    }
}