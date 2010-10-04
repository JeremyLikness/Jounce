using System;
using System.ComponentModel.Composition;

namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     Exports the view model
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,AllowMultiple = false)]
    public class ExportAsViewModelAttribute : ExportAttribute 
    {
        /// <summary>
        ///     Target view model
        /// </summary>
        public ExportAsViewModelAttribute(string viewModelType) : base(typeof(IViewModel))
        {
            ViewModelType = viewModelType;
        }

        /// <summary>
        ///     The type of view model
        /// </summary>
        public string ViewModelType { get; private set; }
    }
}
