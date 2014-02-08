using System;
using System.ComponentModel.Composition;
using Jounce.Desktop.Core.ViewModel;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     Exports the view model
    /// </summary>
    /// <remarks>
    /// Use this attribute to notify Jounce of a view model
    /// </remarks>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,AllowMultiple = false)]
    public class ExportAsViewModelAttribute : ExportAttribute 
    {
        /// <summary>
        ///     Use the type as the tag (full name of the type)
        /// </summary>
        /// <param name="viewModelType">The view model type</param>
        public ExportAsViewModelAttribute(Type viewModelType)
            : this(viewModelType.FullName)
        {
        }

        /// <summary>
        /// Use a user-specified tag name 
        /// </summary>
        public ExportAsViewModelAttribute(string viewModelType) : base(typeof(IViewModel))
        {
            ViewModelType = viewModelType;
        }

        /// <summary>
        /// The tag for the view model
        /// </summary>
        public string ViewModelType { get; private set; }
    }
}
