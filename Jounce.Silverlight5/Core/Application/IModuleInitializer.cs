namespace Jounce.Core.Application
{
    /// <summary>
    /// Use the module initializer to run code when a new 
    /// module loads via the deployment catalog - simply
    /// define and export it, and it will run
    /// </summary>
    public interface IModuleInitializer
    {
        bool Initialized { get; }
        void Initialize();
    }
}