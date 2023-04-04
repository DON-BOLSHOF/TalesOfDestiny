using System;
using UnityEngine;

namespace Panels
{
    public class AbstractStateUtil : MonoBehaviour
    {
        public Action<bool> OnChangeState;

        public void TakeAdditivelyPanelSubscribes(AbstractPanelUtil panel)
        {
            OnChangeState += panel.OnChangeState;
        }
    }
}