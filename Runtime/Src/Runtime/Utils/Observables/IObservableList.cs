using System;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Utils.Observables
{
    public interface IObservableListReader<T> : IEnumerable<T>
    {
        /// <summary>
        /// Requests this list as a <see cref="IReadOnlyList{T}"/>
        /// </summary>
        IReadOnlyList<T> ReadOnlyList { get; }
        
        /// <summary>
        /// Gets list count
        /// </summary>
        int Count { get; }
        
        /// <inheritdoc cref="List{T}.Contains"/>
        bool Contains(T value);

        /// <inheritdoc cref="List{T}.IndexOf(T)"/>
        int IndexOf(T value);

        /// <summary>
        /// Observes this list with a given action
        /// Update action calls back with index, previous value and current value
        /// </summary>
        void Observe(Action<int, T, T> onUpdate);
        
        /// <summary>
        /// Observes this list with given action, and invokes update immediately
        /// </summary>
        void InvokeObserve(int index, Action<int, T, T> onUpdate);

        /// <summary>
        /// Invokes all update actions with the current values
        /// </summary>
        void InvokeUpdate();

        /// <summary>
        /// Stops observing this dictionary
        /// </summary>
        void StopObserving(Action<int, T, T> onUpdate);
        
        /// <summary>
        /// Stops observing this list from all the given subscriber actions
        /// </summary>
        void StopObservingAll(object subscriber = null);

        /// <summary>
        /// Removes all currently subscribed observers
        /// </summary>
        void RemoveAllObservers();
    }

    public interface IObservableList<T> : IObservableListReader<T>
    {
        /// <summary>
        /// Gets/sets data at index
        /// </summary>
        T this[int index] { get; set; }

        /// <inheritdoc cref="List{T}.Add"/>
        void Add(T data);

        /// <inheritdoc cref="List{T}.Remove"/>
        void Remove(T data);

        /// <inheritdoc cref="List{T}.RemoveAt"/>
        void RemoveAt(int index);

        /// <inheritdoc cref="List{T}.Clear"/>
        void Clear();
    }
}
