using System;
using UnityEngine;
using Utils.Disposables;

namespace Panels
{
    public class AbstractStateUtil : MonoBehaviour
    {
        public Action<bool> OnChangeState;

        public void TakeAdditivelyPanelSubscribes(AbstractPanelUtil panel)
        {
            OnChangeState += panel.OnChangeState;
        }

        public IDisposable SubscribeOnChange(Action<bool> call)
        {
            OnChangeState += call;
            return new ActionDisposable(() => OnChangeState -= call);
        }
    }
}