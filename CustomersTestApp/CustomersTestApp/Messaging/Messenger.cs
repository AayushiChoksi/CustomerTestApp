using System;
using System.Collections.Generic;
using CustomersTestApp.ViewModels;

namespace CustomersTestApp.Messaging
{
    public class Messenger
    {
        private static Messenger _instance;
        public static Messenger Instance => _instance ??= new Messenger();

        private readonly Dictionary<Type, List<Action<object>>> _subscribers = new();

        public void Register<TMessage>(Action<TMessage> action)
        {
            var messageType = typeof(TMessage);
            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<Action<object>>();
            }
            _subscribers[messageType].Add(o => action((TMessage)o));
        }

        public void Send<TMessage>(TMessage message)
        {
            var messageType = typeof(TMessage);
            if (_subscribers.ContainsKey(messageType))
            {
                foreach (var action in _subscribers[messageType])
                {
                    action(message);
                }
            }
        }
    }

    public class CustomerAddedMessage
    {
        public CustomerViewModel Customer { get; }
        public CustomerAddedMessage(CustomerViewModel customer)
        {
            Customer = customer;
        }
    }

    public class CustomerRemovedMessage
    {
        public CustomerViewModel Customer { get; }
        public CustomerRemovedMessage(CustomerViewModel customer)
        {
            Customer = customer;
        }
    }
    public class CustomerUpdatedMessage
    {
        public CustomerViewModel Customer { get; }

        public CustomerUpdatedMessage(CustomerViewModel customer)
        {
            Customer = customer;
        }
    }
}
