using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Export a view 
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
    public class ExportAsViewAttribute : ExportAttribute 
    {
        public ExportAsViewAttribute(string viewType) : base(typeof(UserControl))
        {
            ExportedViewType = viewType;
            IsShell = false;
            Category = string.Empty;
            CommandName = string.Empty;
            ToolTip = string.Empty;
        }

        /// <summary>
        ///     The view type
        /// </summary>
        public string ExportedViewType { get; private set; }

        public bool IsShell { get; set; }

        public string Category { get; set; }

        public string CommandName { get; set; }

        public string ToolTip { get; set; }
    }
}
