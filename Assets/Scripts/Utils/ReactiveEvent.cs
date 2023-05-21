using System;
using UniRx;

namespace Utils
{
    public class ReactiveEvent<T>//Прослойка - нам не нужен весь функционал ReactiveCommand
    {
        private ReactiveCommand<T> _event = new ReactiveCommand<T>();
        
        public void Execute(T data)
        {
            _event.Execute(data);
        }

        public IDisposable Subscribe(Action<T> action)
        {
            return _event.Subscribe(action);
        }
    }  
    
    public class ReactiveEvent//Прослойка - нам не нужен весь функционал ReactiveCommand
    {
        private ReactiveCommand _event = new ReactiveCommand();

        public void Execute()
        {
            _event.Execute();
        }

        public IDisposable Subscribe(Action action)
        {
            return _event.Subscribe(_ => action());
        }
    }
}