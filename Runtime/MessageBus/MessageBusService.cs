using System;
using System.Collections;
using System.Collections.Generic;

namespace Potency.Services.Runtime.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        // Structure Dic<MessageType, Dic<Subscriber, List<Subscriptions>>
        private readonly Dictionary<Type, IDictionary<object, IList>> _subscriptions = new();

        public void Publish<T>(T message) where T : IMessage
        {
            if (!_subscriptions.TryGetValue(typeof(T), out var subscriptionsDic))
            {
                return;
            }

            var subscriptionsCopy = new IList[subscriptionsDic.Count];
            
            subscriptionsDic.Values.CopyTo(subscriptionsCopy, 0);

            for (var i = 0; i < subscriptionsCopy.Length; i++)
            {
                var actions = (List<Action<T>>) subscriptionsCopy[i];

                for (var index = 0; index < actions.Count; index++)
                {
                    actions[index](message);
                }
            }
        }

        public void Subscribe<T>(Action<T> action) where T : IMessage
        {
            var messageType = typeof(T);
            var subscriber = action.Target;
            
            if (!_subscriptions.TryGetValue(messageType, out var subscriptionObjects))
            {
                subscriptionObjects = new Dictionary<object, IList>();
                _subscriptions.Add(messageType, subscriptionObjects);
            }

            if (!subscriptionObjects.TryGetValue(subscriber, out IList actions))
            {
                actions = new List<Action<T>>();
                subscriptionObjects.Add(subscriber, actions);
            }
            
            actions.Add(action);
        }
        
        public void Unsubscribe<T>() where T : IMessage
        {
            _subscriptions.Remove(typeof(T));
        }

        public void Unsubscribe<T>(Action<T> action) where T : IMessage
        {
            var type = typeof(T);
            var subscriber = action.Target;

            if (!_subscriptions.TryGetValue(type, out var subscriptionObjects) || 
                !subscriptionObjects.TryGetValue(subscriber, out var actions))
            {
                return;
            }

            actions.Remove(action);

            if (actions.Count == 0)
            {
                subscriptionObjects.Remove(subscriber);
            }

            if (subscriptionObjects.Count == 0)
            {
                _subscriptions.Remove(type);
            }
        }

        public void UnsubscribeAll(object subscriber = null)
        {
            if (subscriber == null)
            {
                _subscriptions.Clear();
                return;
            }
            
            foreach (var subscriptionObjects in _subscriptions.Values)
            {
                if (subscriptionObjects.ContainsKey(subscriber))
                {
                    subscriptionObjects.Remove(subscriber);
                }
            }
        }
    }
}
