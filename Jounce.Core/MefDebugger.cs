using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using Jounce.Core.Application;

namespace Jounce.Core
{
    /// <summary>
    ///     Helper class for debugging MEF
    /// </summary>
    public class MefDebugger
    {
        private readonly string _name;

        const string MSG_ADD_EXPORT = "Added Export:";
        const string MSG_CHANGE_EXPORT = "Removed Export:";
        const string MSG_CHANGE_CONTRACT = "Changed contracts:";
        const string DBG_MEF_CATALOG = "MEF: Found catalog: {0}";
        const string DBG_MEF_PART = "MEF: Found part: {0}";
        const string DBG_MEF_WITH_KEY = "   With key: {0} = {1}";
        const string DBG_MEF_WITH_EXPORT = "   With export:";
        const string DBG_MEF_WITH_IMPORT = "   With import: {0}";
        const string DBG_MEF_KEY = "      With key: {0} = {1}";                

        private readonly CompositionContainer _container;
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="container">The container to debug</param>
        /// <param name="logger">The logger</param>
        public MefDebugger(CompositionContainer container, ILogger logger)
        {
            _name = GetType().FullName;
            _container = container;
            _logger = logger;
            _container.ExportsChanged += ExportsChanged;
            _DebugCatalog((AggregateCatalog)container.Catalog);
        }

        private void ExportsChanged(object sender, ExportsChangeEventArgs args)
        {
            try
            {
                if (args.AddedExports != null)
                {
                    _ParseExports(MSG_ADD_EXPORT, args.AddedExports);
                }

                if (args.RemovedExports != null)
                {
                    _ParseExports(MSG_CHANGE_EXPORT, args.RemovedExports);
                }

                if (args.ChangedContractNames != null)
                {
                    var first = true;
                    foreach (var contract in args.ChangedContractNames)
                    {
                        if (first)
                        {
                            _logger.Log(LogSeverity.Verbose, _name, MSG_CHANGE_CONTRACT);
                            first = false;
                        }
                        _logger.LogFormat(LogSeverity.Verbose, _name, " ==>{0}", contract);
                    }
                }                
            }
            catch(Exception ex)
            {
                _logger.LogFormat(LogSeverity.Warning, _name, ex.Message);
            }
        }

        /// <summary>
        ///     Debug the catalog
        /// </summary>
        /// <param name="srcCatalog">The source catalog</param>
        private void _DebugCatalog(AggregateCatalog srcCatalog)
        {                        
            foreach (var catalog in srcCatalog.Catalogs)
            {    
                _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_CATALOG, catalog);                    
                
                foreach (var part in catalog.Parts)
                {
                    _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_PART, part);
                    
                    if (part.Metadata != null)
                    {
                        foreach (var key in part.Metadata.Keys)
                        {
                            _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_WITH_KEY, key, part.Metadata[key]);                            
                        }
                    }

                    foreach (var import in part.ImportDefinitions)
                    {
                        _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_WITH_IMPORT, import);                        
                    }

                    _ParseExports(DBG_MEF_WITH_EXPORT, part.ExportDefinitions);

                    foreach (var export in part.ExportDefinitions)
                    {
                        _logger.LogFormat(LogSeverity.Verbose, _name, "{0} {1}", DBG_MEF_WITH_EXPORT, export);
                        
                        if (export.Metadata == null) continue;

                        foreach (var key in export.Metadata.Keys)
                        {
                            _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_KEY, key, export.Metadata[key]);                         
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Parse the exports
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="exports"></param>
        private void _ParseExports(string tag, IEnumerable<ExportDefinition> exports)
        {
            foreach (var export in exports)
            {
                _logger.LogFormat(LogSeverity.Verbose, _name, "{0} {1}", tag, export);
                
                if (export.Metadata == null) continue;

                foreach (var key in export.Metadata.Keys)
                {
                    _logger.LogFormat(LogSeverity.Verbose, _name, DBG_MEF_KEY, key, export.Metadata[key]);                    
                }
            }
        }

        public void Close()
        {
            _logger.Log(LogSeverity.Information, GetType().FullName, "MEF Debugger shutting down.");
            _container.ExportsChanged -= ExportsChanged;
        }
    }    
}
