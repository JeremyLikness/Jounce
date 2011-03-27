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
        /// <summary>
        ///     Constructor to use type
        /// </summary>
        /// <param name="viewType">Default to type</param>
        public ExportAsViewAttribute(Type viewType)
            : this(viewType.FullName)
        {
        }

        /// <summary>
        ///     Constructor to use tag
        /// </summary>
        /// <param name="viewType">The tag</param>
        public ExportAsViewAttribute(string viewType) : base(typeof(UserControl))
        {
            ExportedViewType = viewType;
            IsShell = false;
            Category = string.Empty;
            MenuName = string.Empty;
            ToolTip = string.Empty;
        }

        /// <summary>
        ///     The view type
        /// </summary>
        public string ExportedViewType { get; private set; }

        public bool IsShell { get; set; }

        public string Category { get; set; }

        public string MenuName { get; set; }

        public string ToolTip { get; set; }
    }
}
