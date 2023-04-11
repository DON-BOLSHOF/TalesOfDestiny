using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components
{
    public class MouseEnterComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action OnEnter;
        public event Action OnExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
        }
    }
}