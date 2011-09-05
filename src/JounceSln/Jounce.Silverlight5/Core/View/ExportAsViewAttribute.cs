using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Export a view 
    /// </summary>
    /// <remarks>
    /// Use this attribute to export a view. If you export using the type of the view, the full name
    /// for he type will be used as the tag for the view.
    /// </remarks>
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
        /// The view type
        /// </summary>
        public string ExportedViewType { get; private set; }

        /// <summary>
        /// Set to true for the view that should serve as the main shell
        /// </summary>
        public bool IsShell { get; set; }

        /// <summary>
        /// Optional category, for organizing views together
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// If you wish to place the view on a menu, use this to provide a user-friendly name
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// A tool tip for providing more information about the view
        /// </summary>
        public string ToolTip { get; set; }
    }
}
