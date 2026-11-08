using System;
using System.Collections.Generic;
using UnityEngine;

namespace ValVertII.Core
{
    public sealed class IdRegistry : IAppService
    {
        private readonly Dictionary<string, StableId> _items =
            new Dictionary<string, StableId>(StringComparer.Ordinal);

        public bool IsInitialized { get; private set; }

        public IReadOnlyDictionary<string, StableId> Items => _items;

        public void Initialize()
        {
            _items.Clear();
            IsInitialized = true;
        }

        public bool Register(StableId item)
        {
            EnsureInitialized();

            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return false;

            if (_items.TryGetValue(item.Id, out StableId existing) &&
                existing != item)
                return false;

            _items[item.Id] = item;
            return true;
        }

        public void Unregister(StableId item)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return;

            if (_items.TryGetValue(item.Id, out StableId existing) &&
                existing == item)
            {
                _items.Remove(item.Id);
            }
        }

        public bool TryGet(string id, out StableId item)
        {
            return _items.TryGetValue(id, out item);
        }

        public void RebuildFromLoadedObjects()
        {
            EnsureInitialized();
            _items.Clear();

            StableId[] all =
                UnityEngine.Object.FindObjectsByType<StableId>();

            foreach (StableId item in all)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.Id))
                    continue;

                if (!_items.ContainsKey(item.Id))
                    _items.Add(item.Id, item);
            }
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("IdRegistry non initialisé.");
        }

        public void Shutdown()
        {
            _items.Clear();
            IsInitialized = false;
        }
    }
}