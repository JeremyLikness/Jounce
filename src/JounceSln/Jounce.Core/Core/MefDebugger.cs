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

        private readonly string _msgAddExport = Resources.MefDebugger__msgAddExport_Added_Export;
        private readonly string _msgChangedExport = Resources.MefDebugger__msgChangedExport_Removed_Export;
        private readonly string _msgChangeContract = Resources.MefDebugger__msgChangeContract_Changed_contracts;
        private readonly string _dbgMefCatalog = Resources.MefDebugger__dbgMefCatalog_MEF__Found_catalog;
        private readonly string _dbgMefPart = Resources.MefDebugger__dbgMefPart_MEF__Found_part;
        private readonly string _dbgMefWithKey = Resources.MefDebugger__dbgMefWithKey;
        private readonly string _dbgMefWithExport = Resources.MefDebugger__dbgMefWithExport_With_export;
        private readonly string _dbgMefWithImport = Resources.MefDebugger__dbgMefWithImport_With_import;
        private readonly string _dbgMefKey = Resources.MefDebugger__dbgMefKey;                

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
                    _ParseExports(_msgAddExport, args.AddedExports);
                }

                if (args.RemovedExports != null)
                {
                    _ParseExports(_msgChangedExport, args.RemovedExports);
                }

                if (args.ChangedContractNames != null)
                {
                    var first = true;
                    foreach (var contract in args.ChangedContractNames)
                    {
                        if (first)
                        {
                            _logger.Log(LogSeverity.Verbose, _name, _msgChangeContract);
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
                _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefCatalog, catalog);                    
                
                foreach (var part in catalog.Parts)
                {
                    _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefPart, part);
                    
                    if (part.Metadata != null)
                    {
                        foreach (var key in part.Metadata.Keys)
                        {
                            _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefWithKey, key, part.Metadata[key]);                            
                        }
                    }

                    foreach (var import in part.ImportDefinitions)
                    {
                        _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefWithImport, import);                        
                    }

                    _ParseExports(_dbgMefWithExport, part.ExportDefinitions);

                    foreach (var export in part.ExportDefinitions)
                    {
                        _logger.LogFormat(LogSeverity.Verbose, _name, "{0} {1}", _dbgMefWithExport, export);
                        
                        if (export.Metadata == null) continue;

                        foreach (var key in export.Metadata.Keys)
                        {
                            _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefKey, key, export.Metadata[key]);                         
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
                    _logger.LogFormat(LogSeverity.Verbose, _name, _dbgMefKey, key, export.Metadata[key]);                    
                }
            }
        }

        public void Close()
        {
            _logger.Log(LogSeverity.Information, GetType().FullName, Resources.MefDebugger_Close_MEF_Debugger_shutting_down);
            _container.ExportsChanged -= ExportsChanged;
        }
    }    
}
