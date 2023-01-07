using System;

namespace Utils.Interfaces
{
    public interface IClickable<T>
    {
        public event Action<T> IClicked;
        
        public event Action OnClick;

        public void Click();
    }
}