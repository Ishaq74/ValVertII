namespace ValVertII.Core
{
    public sealed class AppModeManager : IAppService
    {
        private readonly InputRouter _input;

        public bool IsInitialized { get; private set; }
        public AppInputMode CurrentMode => _input.CurrentMode;

        public AppModeManager(InputRouter input)
        {
            _input = input;
        }

        public void Initialize()
        {
            _input.Initialize();
            IsInitialized = true;
            SetMode(AppInputMode.UI);
        }

        public void SetMode(AppInputMode mode)
        {
            if (!IsInitialized && mode != AppInputMode.UI)
                throw new System.InvalidOperationException(
                    "AppModeManager non initialisé.");

            _input.SetMode(mode);
        }

        public void Shutdown()
        {
            if (!IsInitialized) return;

            _input.Shutdown();
            IsInitialized = false;
        }
    }
}