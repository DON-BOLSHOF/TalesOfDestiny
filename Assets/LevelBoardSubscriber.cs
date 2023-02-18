using System;
using System.Collections.Generic;
using Panels;
using UnityEngine;
using Utils.Disposables;
using Utils.Interfaces;

public class LevelBoardSubscriber : MonoBehaviour //Чисто для связывания ссылок внутри LevelBoard и всех панелек.
{
    private Action<bool> OnChangeState;

    public void BoundPanelsUtil(IEnumerable<AbstractTextPanelUtil> checks, List<ISubscriber> subscribers = null)//list нужен для динамических сабскрайберов.
    {
        subscribers?.ForEach((s) => s.Subscribe());

        foreach (var check in checks)
        {
            check.OnChangeState += OnChangeState;
        }
    }
    
    public IDisposable Subscribe(Action<bool> call)
    {
        OnChangeState += call;
        return new ActionDisposable(() => OnChangeState -= call);
    }
}
