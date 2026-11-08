namespace ValVertII.Core
{
    public interface IAppService
    {
        bool IsInitialized { get; }
        void Initialize();
        void Shutdown();
    }
}
