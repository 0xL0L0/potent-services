using System;

namespace Potency.Services.Runtime.MessageBus
{
    public interface IMessageBusService
    {
        void Publish<T>(T message) where T : IMessage;

        void Subscribe<T>(Action<T> action) where T : IMessage;

        void Unsubscribe<T>(Action<T> action) where T : IMessage;

        void Unsubscribe<T>() where T : IMessage;

        void UnsubscribeAll(object subscriber = null);
    }
}