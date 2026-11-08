using System;

namespace ValVertII.Core
{
    public sealed class WorldSession : IAppService
    {
        public bool IsInitialized { get; private set; }
        public bool IsRunning { get; private set; }
        public string SessionId { get; private set; }
        public string StartingSectorId { get; private set; }
        public DateTime StartedUtc { get; private set; }

        public void Initialize()
        {
            IsInitialized = true;
        }

        public void BeginNewSession(string startingSectorId)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(startingSectorId))
                throw new ArgumentException("Un ID de secteur de départ est obligatoire.");

            SessionId = Guid.NewGuid().ToString("N");
            StartingSectorId = startingSectorId.Trim();
            StartedUtc = DateTime.UtcNow;
            IsRunning = true;
        }

        public void RestoreSession(
            string sessionId,
            string startingSectorId,
            DateTime startedUtc)
        {
            EnsureInitialized();

            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("SessionId est obligatoire.");

            if (string.IsNullOrWhiteSpace(startingSectorId))
                throw new ArgumentException("StartingSectorId est obligatoire.");

            SessionId = sessionId;
            StartingSectorId = startingSectorId;
            StartedUtc = startedUtc;
            IsRunning = true;
        }

        public void EndSession()
        {
            IsRunning = false;
            SessionId = null;
            StartingSectorId = null;
            StartedUtc = default;
        }

        private void EnsureInitialized()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("WorldSession non initialisée.");
        }

        public void Shutdown()
        {
            EndSession();
            IsInitialized = false;
        }
    }
}