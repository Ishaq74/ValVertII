using UnityEngine;
using UnityEngine.SceneManagement;

namespace ValVertII.Core
{
    public sealed class DiagnosticsService : IAppService
    {
        private float _startedAt;

        public bool IsInitialized { get; private set; }
        public float UptimeSeconds =>
            IsInitialized ? Time.realtimeSinceStartup - _startedAt : 0f;

        public float FramesPerSecond { get; private set; }
        public string ActiveSceneName => SceneManager.GetActiveScene().name;

        public void Initialize()
        {
            _startedAt = Time.realtimeSinceStartup;
            IsInitialized = true;
        }

        public void Tick(float unscaledDeltaTime)
        {
            if (!IsInitialized || unscaledDeltaTime <= 0f) return;
            FramesPerSecond = 1f / unscaledDeltaTime;
        }

        public void Shutdown()
        {
            IsInitialized = false;
        }
    }
}