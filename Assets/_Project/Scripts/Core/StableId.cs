using UnityEngine;

namespace ValVertII.Core
{
    [DisallowMultipleComponent]
    public sealed class StableId : MonoBehaviour
    {
        [SerializeField] private string id;

        public string Id => id;

        private void OnEnable()
        {
            PersistentRoot root = PersistentRoot.Instance;

            if (root != null && root.IsReady &&
                !root.IdRegistry.Register(this))
            {
                root.Logger.Warning(
                    $"ID vide ou déjà utilisé : '{id}' sur '{name}'.");
            }
        }

        private void OnDisable()
        {
            PersistentRoot root = PersistentRoot.Instance;

            if (root != null && root.IsReady)
                root.IdRegistry.Unregister(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrWhiteSpace(id) && id != id.Trim())
                id = id.Trim();
        }
#endif
    }
}