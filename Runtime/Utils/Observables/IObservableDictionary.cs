using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Potency.Services.Runtime.Utils.Observables
{
    public interface IObservableDictionaryReader<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Gets dictionary value at key
        /// </summary>
        TValue this[TKey key] { get; }
        
        /// <summary>
        /// Requests this dictionary as a <see cref="IReadOnlyDictionary{TKey,TValue}"/>
        /// </summary>
        ReadOnlyDictionary<TKey, TValue> ReadOnlyDictionary { get; }
        
        /// <summary>
        /// Gets dictionary count
        /// </summary>
        int Count { get; }
        
        /// <inheritdoc cref="Dictionary{TKey,TValue}.TryGetValue" />
        bool TryGetValue(TKey key, out TValue value);

        /// <inheritdoc cref="Dictionary{TKey,TValue}.ContainsKey" />
        bool ContainsKey(TKey key);
        
        /// <remarks>
        /// Invokes all update methods on all keys of this dictionary, with current values
        /// </remarks>
        void InvokeUpdate();
        
        /// <summary>
        /// Observes this dictionary with the given action
        /// </summary>
        void Observe(Action<TKey, TValue, TValue> onUpdate);
        
        /// <summary>
        /// Observes this dictionary with given action, and invokes update immediately
        /// </summary>
        void InvokeObserve(TKey key, Action<TKey, TValue, TValue> onUpdate);

        /// <summary>
        /// Stops observing this dictionary with the given <paramref name="onUpdate"/> of any data changes
        /// </summary>
        void StopObserving(Action<TKey, TValue, TValue> onUpdate);
        
        /// <summary>
        /// Stops observing this dictionary from all the given subscriber actions
        /// </summary>
        void StopObservingAll(object subscriber = null);

        /// <summary>
        /// Removes all currently subscribed observers
        /// </summary>
        void RemoveAllObservers();
    }

    public interface IObservableDictionary<TKey, TValue> : IObservableDictionaryReader<TKey, TValue>
    {
        /// <summary>
        /// Gets/sets dictionary value at key
        /// </summary>
        new TValue this[TKey key] { get; set; }

        /// <inheritdoc cref="Dictionary{TKey,TValue}.Add" />
        void Add(TKey key, TValue value);

        /// <inheritdoc cref="Dictionary{TKey,TValue}.Remove" />
        bool Remove(TKey key);
    }
}
