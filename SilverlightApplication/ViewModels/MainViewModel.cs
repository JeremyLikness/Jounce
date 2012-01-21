using Jounce.Core.ViewModel;

namespace SilverlightApplication.ViewModels
{
    /// <summary>
    /// Sample view model showing design-time resolution of data
    /// </summary>
    [ExportAsViewModel(typeof (MainViewModel))]
    public class MainViewModel : BaseViewModel
    {
        public string Welcome
        {
            get { return InDesigner ? "Jounce Design-time View" : "Welcome to Jounce."; }
        }
    }
}