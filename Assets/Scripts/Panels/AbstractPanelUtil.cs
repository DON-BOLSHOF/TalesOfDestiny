using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Panels
{
    public abstract class AbstractPanelUtil : MonoBehaviour
    {
        public Action<bool> OnChangeState;
        
        public abstract void Show();

        public abstract void Exit();

        public void TakeAdditivelyPanelSubscribes(AbstractPanelUtil panel)
        {
            OnChangeState += panel.OnChangeState;
        }
    }
}