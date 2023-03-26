using System;
using System.Collections.Generic;

namespace Utils.Disposables
{
    public class DisposeHolder : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public void Retain(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            foreach (var dispose in _disposables)
            {
                dispose.Dispose();
            }
            
            _disposables.Clear();
        }
    }
}