// EventManager.cs
using Godot;
using System;
using System.Collections.Generic;

namespace core
{
    public partial class EventManager : Node
    {
        // Store listeners by event type
        private Dictionary<Type, List<Delegate>> _eventListeners = new();

        // Subscribe to a specific event type
        public void Subscribe<TEvent>(Action<TEvent> listener) where TEvent : events.IEvent
        {
            Type eventType = typeof(TEvent);
            if (!_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = new List<Delegate>();
            }
            _eventListeners[eventType].Add(listener);
        }

        // Publish a specific event type
        public void Publish<TEvent>(TEvent eventData) where TEvent : events.IEvent
        {
            Type eventType = typeof(TEvent);
            if (_eventListeners.ContainsKey(eventType))
            {
                foreach (var listener in _eventListeners[eventType])
                {
                    if (listener is Action<TEvent> typedListener)
                    {
                        typedListener?.Invoke(eventData);
                    }
                }
            }
        }
    }
}
