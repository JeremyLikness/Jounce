using System;

namespace Jounce.Framework.Exceptions
{
    /// <summary>
    ///     Exception download a deployment catalog
    /// </summary>
    public class DeploymentCatalogDownloadException : Exception
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="innerException">The inner exception</param>
        public DeploymentCatalogDownloadException(Exception innerException) : base(Resources.DeploymentService_DeploymentCatalogDownloadCompleted_ModuleNotFound, innerException)
        {
            
        }
    }
}