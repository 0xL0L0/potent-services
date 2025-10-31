using System;

namespace Potency.Services.Runtime.Utils.Observables
{

    public interface IObservableFieldReader<out T>
    {
        /// <summary>
        /// The field value
        /// </summary>
        T Value { get; }
        
        /// <summary>
        /// Observes this field with the given action.
        /// Update action calls back with previous and current values
        /// </summary>
        void Observe(Action<T, T> onUpdate);
        
        /// <summary>
        /// Observer this field with given action, and invokes update immediately
        /// </summary>
        void InvokeObserve(Action<T, T> onUpdate);
    
        /// <summary>
        /// Stops observing this field
        /// </summary>
        void StopObserving(Action<T, T> onUpdate);
        
        /// <summary>
        /// Stops observing this field from all the given subscriber actions
        /// </summary>
        void StopObservingAll(object subscriber = null);
        
        /// <summary>
        /// Removes all currently subscribed observers
        /// </summary>
        void RemoveAllObservers();
        
        /// <remarks>
        /// Invokes all update actions with the current value
        /// </remarks>
        void InvokeUpdate();
    }
    
    public interface IObservableField<T> : IObservableFieldReader<T>
    {
        /// <summary>
        /// The field value
        /// </summary>
        new T Value { get; set; }
    }
}