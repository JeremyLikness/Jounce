using System.ComponentModel.Composition.Hosting;
using System;

namespace Jounce.Core.Application
{
    /// <summary>
    ///     Request the xap
    /// </summary>
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
        ///     The main catalog
        /// </summary>
        AggregateCatalog Catalog { get; set; }

        /// <summary>
        ///     Logger
        /// </summary>
        ILogger Logger { get; set; }
    }
}
