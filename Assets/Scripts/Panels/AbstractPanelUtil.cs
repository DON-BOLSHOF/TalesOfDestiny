using System;
using UnityEngine;

namespace Panels
{
    public abstract class AbstractPanelUtil : MonoBehaviour
    {
        public Action<bool> OnChangeState;
        
        public abstract void Show();
        
        public abstract void Exit();

        public void TakePanelSubscribes(AbstractPanelUtil panel)
        {
            OnChangeState = panel.OnChangeState;
        }
    }
}