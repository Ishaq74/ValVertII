using System;
using System.Collections.Generic;
using UnityEngine;

namespace ValVertII.Core
{
    public sealed class WorldEventBus : IAppService
    {
        private readonly Dictionary<Type, Delegate> _handlers =
            new Dictionary<Type, Delegate>();

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            _handlers.Clear();
            IsInitialized = true;
        }

        public IDisposable Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("WorldEventBus non initialisé.");

            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type type = typeof(TEvent);

            if (_handlers.TryGetValue(type, out Delegate current))
                _handlers[type] = Delegate.Combine(current, handler);
            else
                _handlers.Add(type, handler);

            return new Subscription(() => Unsubscribe(handler));
        }

        public void Publish<TEvent>(TEvent worldEvent)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("WorldEventBus non initialisé.");

            if (!_handlers.TryGetValue(typeof(TEvent), out Delegate handlers))
                return;

            if (handlers is Action<TEvent> typed)
            {
                foreach (Action<TEvent> callback in typed.GetInvocationList())
                {
                    try
                    {
                        callback.Invoke(worldEvent);
                    }
                    catch (Exception exception)
                    {
                        Debug.LogException(exception);
                    }
                }
            }
        }

        private void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            Type type = typeof(TEvent);

            if (!_handlers.TryGetValue(type, out Delegate current))
                return;

            Delegate updated = Delegate.Remove(current, handler);

            if (updated == null)
                _handlers.Remove(type);
            else
                _handlers[type] = updated;
        }

        public void Shutdown()
        {
            _handlers.Clear();
            IsInitialized = false;
        }

        private sealed class Subscription : IDisposable
        {
            private Action _dispose;

            public Subscription(Action dispose) => _dispose = dispose;

            public void Dispose()
            {
                _dispose?.Invoke();
                _dispose = null;
            }
        }
    }

    public readonly struct WorldStartedEvent
    {
        public readonly string StartingSectorId;

        public WorldStartedEvent(string startingSectorId)
        {
            StartingSectorId = startingSectorId;
        }
    }
}