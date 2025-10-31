using System;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Utils.Observables
{
    /// <inheritdoc cref="IObservableField{T}"/>
    public class ObservableField<T> : IObservableField<T>
    {
        private readonly List<Action<T, T>> _updateActions = new List<Action<T, T>>();

        private T _value;
        
        public T Value
        {
            get => _value;
            set
            {
                var previousValue = _value;
				
                _value = value;
                InvokeUpdate(previousValue);
            }
        }
    
        public ObservableField()
        {
            _value = default;
        }
 
        public ObservableField(T initialValue)
        {
            _value = initialValue;
        }
        
        public void Observe(Action<T, T> onUpdate)
        {
            _updateActions.Add(onUpdate);
        }

        public void InvokeObserve(Action<T, T> onUpdate)
        {
            Observe(onUpdate);
            onUpdate.Invoke(Value, Value);
        }

        public void StopObserving(Action<T, T> onUpdate)
        {
            _updateActions.Remove(onUpdate);
        }

        public void StopObservingAll(object subscriber = null)
        {
            if (subscriber == null)
            {
                _updateActions.Clear();
                return;
            }

            for (var i = _updateActions.Count - 1 ; i > -1; i--)
            {
                if (_updateActions[i].Target == subscriber)
                {
                    _updateActions.RemoveAt(i);
                }
            }
        }

        public void InvokeUpdate()
        {
            InvokeUpdate(Value);
        }
        
        public void RemoveAllObservers()
        {
            _updateActions.Clear();
        }
        
        private void InvokeUpdate(T previousValue)
        {
            foreach (var action in _updateActions)
            {
                action.Invoke(previousValue, Value);
            }
        }
    }
}