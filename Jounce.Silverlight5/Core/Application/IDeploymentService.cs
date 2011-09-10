using System.ComponentModel.Composition.Hosting;
using System;

namespace Jounce.Core.Application
{
    /// <summary>
    ///     Service for downloading XAP files
    /// </summary>
    /// <remarks>
    /// This is a core interface that is explicitly implemented by the application service. Use it to get a reference to the main 
    /// <see cref="AggregateCatalog"/> for the application and the MEF <see cref="CompositionContainer"/>. Import it to 
    /// request XAP files with an optional callback once the XAP has been loaded
    /// </remarks>
    public interface IDeploymentService
    {
        /// <summary>
        ///     Request a xap to be downloaded and integrated
        /// </summary>
        /// <param name="xapName">The name of the XAP to download</param>
        void RequestXap(string xapName);

        /// <summary>
        ///     Request a xap to be downloaded and integrated
        /// </summary>
        /// <param name="xapName">The name of the XAP to download</param>
        /// <param name="xapLoaded">Callback when xap is loaded</param>
        void RequestXap(string xapName, Action<Exception> xapLoaded);

        /// <summary>
        ///  Request a xap to be downloaded and integrated
        /// </summary>
        /// <param name="xapName">The name of the xap to download</param>
        /// <param name="xapLoaded">Callback when xap is loaded</param>
        /// <param name="xapProgress">Callback to report xap progress (bytes received, percent complete, total bytes)</param>
        void RequestXap(string xapName, Action<Exception> xapLoaded,
                        Action<long, int, long> xapProgress);
       
        /// <summary>
        /// The main catalog
        /// </summary>
        AggregateCatalog Catalog { get; set; }

        /// <summary>
        /// The main container
        /// </summary>
        CompositionContainer Container { get; set; }

        /// <summary>
        /// Logger
        /// </summary>
        ILogger Logger { get; set; }
    }
}
