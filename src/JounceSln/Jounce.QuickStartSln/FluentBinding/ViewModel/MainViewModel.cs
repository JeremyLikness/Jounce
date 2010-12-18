using FluentBinding.Common;
using Jounce.Core.ViewModel;

namespace FluentBinding.ViewModel
{
    [ExportAsViewModel("MainViewModel")]
    public class MainViewModel : BaseViewModel, IMainViewModel 
    {
        public string Welcome
        {
            get { return "Welcome to Jounce."; }
        }
    }
}