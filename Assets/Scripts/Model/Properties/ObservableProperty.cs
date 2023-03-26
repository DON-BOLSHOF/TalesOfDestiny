using System;
using UnityEngine;
using Utils.Disposables;

namespace Model.Properties
{
    [Serializable]
    public class ObservableProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;

        public delegate void OnPropertyChanged(TPropertyType newValue);

        public event OnPropertyChanged OnChanged;

        public ObservableProperty(TPropertyType value)
        {
            _value = value;
        }

        public ObservableProperty()
        {
        }
        
        public void SetBaseValue(TPropertyType value)//Если новое базовое значение, не надо прокидывать подписки.
        {
            _value = value;
        }

        public TPropertyType Value
        {
            get => _value;
            set
            {
                if (_value != null && _value.Equals(value))
                    return;

                _value = value;
                OnChanged?.Invoke(value);
            }
        }
        
        public IDisposable Subscribe(OnPropertyChanged call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public IDisposable SubscribeAndInvoke(OnPropertyChanged call)
        {
            OnChanged += call;
            call(_value);
            return new ActionDisposable(() => OnChanged -= call);
        }
    }
}