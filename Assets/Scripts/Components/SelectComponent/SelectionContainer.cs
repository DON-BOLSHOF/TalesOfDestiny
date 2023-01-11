using System.Collections.Generic;
using UnityEngine;

namespace Components.SelectComponent
{
    public class SelectionContainer : MonoBehaviour
    {
        private List<SelectedGameObject> _list = new List<SelectedGameObject>();

        private void OnEnable()
        {
            _list.AddRange(GetComponentsInChildren<SelectedGameObject>());

            foreach (var gameObject in _list)
            {
                gameObject.ISelected += CheckSelection;
            }
        
            _list[0].ActivateSelection(true);
        }

        private void CheckSelection(SelectedGameObject go)
        {
            foreach (var gameObject in _list)
            { 
                gameObject.ActivateSelection(gameObject == go);
            }
        }
    }
}
