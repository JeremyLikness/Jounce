namespace Jounce.Core.Application
{
    /// <summary>
    /// Use the module initializer to run code when a new 
    /// module loads via the deployment catalog - simply
    /// define and export it, and it will run
    /// </summary>
    public interface IModuleInitializer
    {
        /// <summary>
        /// When set to true, <see cref="Initialize"/> will no longer be called
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Called whenever a new XAP file is loaded or a recomposition takes effect
        /// </summary>
        /// <remarks>
        /// Implement and export this interface to handle initialization for modules when they 
        /// are dynamically loaded
        /// </remarks>
        void Initialize();
    }
}