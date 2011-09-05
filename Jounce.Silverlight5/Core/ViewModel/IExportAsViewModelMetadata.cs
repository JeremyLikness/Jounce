namespace Jounce.Core.ViewModel
{
    /// <summary>
    ///     Meta data for view model exports
    /// </summary>
    public interface IExportAsViewModelMetadata
    {
        /// <summary>
        /// Tag for the view model
        /// </summary>
        string ViewModelType { get; }
    }
}