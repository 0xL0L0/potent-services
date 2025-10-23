using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Potency.Services.Runtime.Utils.Observables
{
    public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>
    {
        private readonly IList<Action<TKey, TValue, TValue>> _updateActions = new List<Action<TKey, TValue, TValue>>();
        
        protected virtual IDictionary<TKey, TValue> Dictionary { get; }
        public ReadOnlyDictionary<TKey, TValue> ReadOnlyDictionary => new ReadOnlyDictionary<TKey, TValue>(Dictionary);
        
        public int Count => Dictionary.Count;
        
        protected ObservableDictionary()
        {
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
        }

        /// <inheritdoc cref="Dictionary{TKey,TValue}.this" />
        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set
            {
                var previousValue = Dictionary[key];

                Dictionary[key] = value;

                InvokeUpdate(key, previousValue);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        
        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        
        public void Add(TKey key, TValue value)
        {
            Dictionary.Add(key, value);
            
            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](key, default, value);
            }
        }

        public bool Remove(TKey key)
        {
            if(!Dictionary.TryGetValue(key, out var value))
            {
                return false;
            }

            Dictionary.Remove(key);

            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](key, value, default);
            }

            return true;
        }
        
        public void InvokeUpdate()
        {
            foreach(var key in Dictionary.Keys)
            {
                InvokeUpdate(key, Dictionary[key]);
            }
        }

        public void Observe(Action<TKey, TValue, TValue> onUpdate)
        {
            _updateActions.Add(onUpdate);
        }

        public void InvokeObserve(TKey key, Action<TKey, TValue, TValue> onUpdate)
        {
            Observe(onUpdate);
            onUpdate.Invoke(key, Dictionary[key], Dictionary[key]);
        }

        public void StopObserving(Action<TKey, TValue, TValue> onUpdate)
        {
            for(var i = _updateActions.Count - 1; i > -1; i--)
            {
                if(_updateActions[i] == onUpdate)
                {
                    _updateActions.RemoveAt(i);
                }
            }
        }

        public void StopObservingAll(object subscriber = null)
        {
            if (subscriber == null)
            {
                _updateActions.Clear();
                return;
            }
            
            for (var i = _updateActions.Count - 1; i > -1; i--)
            {
                if (_updateActions[i].Target == subscriber)
                {
                    _updateActions.RemoveAt(i);
                }
            }
        }

        public void RemoveAllObservers()
        {
            _updateActions.Clear();
        }
        
        private void InvokeUpdate(TKey key, TValue previousValue)
        {
            var value = Dictionary[key];
            
            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](key, previousValue, value);
            }
        }
    }
}
