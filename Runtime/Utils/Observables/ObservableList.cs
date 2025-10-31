using System;
using System.Collections;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Utils.Observables
{
    public class ObservableList<T> : IObservableList<T>
    {
        private readonly IList<Action<int, T, T>> _updateActions = new List<Action<int, T, T>>();
        protected virtual List<T> List { get; }
        
        public T this[int index]
        {
            get => List[index];
            set
            {
                var previousValue = List[index];

                List[index] = value;

                InvokeUpdate(index, previousValue);
            }
        }
        
        public IReadOnlyList<T> ReadOnlyList => List;
        public int Count => List.Count;
        
        protected ObservableList()
        {
        }

        public ObservableList(List<T> list)
        {
            List = list;
        }
        
        public List<T>.Enumerator GetEnumerator()
        {
            return List.GetEnumerator();
        }
        
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return List.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
        
        public bool Contains(T value)
        {
            return List.Contains(value);
        }
        
        public int IndexOf(T value)
        {
            return List.IndexOf(value);
        }
        
        public void Add(T data)
        {
            List.Add(data);

            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](List.Count - 1, default, data);
            }
        }

        public void Remove(T data)
        {
            var idx = List.IndexOf(data);

            if(idx >= 0)
            {
                RemoveAt(idx);
            }
        }

        public void RemoveAt(int index)
        {
            var data = List[index];

            List.RemoveAt(index);

            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](index, data, default);
            }
        }

        public void Clear()
        {
            var data = new List<T>(List);

            List.Clear();

            for(var i = 0; i < _updateActions.Count; i++)
            {
                for(var j = 0; j < data.Count; j++)
                {
                    _updateActions[i](j, data[j], default);
                }
            }
        }

        public void InvokeObserve(int index, Action<int, T, T> onUpdate)
        {
            Observe(onUpdate);
            onUpdate.Invoke(index, List[index], List[index]);
        }

        public void InvokeUpdate()
        {
            for(int i = 0; i < List.Count; i++)
            {
                InvokeUpdate(i, List[i]);
            }
        }
        
        public void Observe(Action<int, T, T> onUpdate)
        {
            _updateActions.Add(onUpdate);
        }

        public void StopObserving(Action<int, T, T> onUpdate)
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

        private void InvokeUpdate(int index, T previousValue)
        {
            var data = List[index];

            for(var i = 0; i < _updateActions.Count; i++)
            {
                _updateActions[i](index, previousValue, data);
            }
        }
        
        private void InvokeUpdate(int index)
        {
            InvokeUpdate(index, List[index]);
        }
    }
}