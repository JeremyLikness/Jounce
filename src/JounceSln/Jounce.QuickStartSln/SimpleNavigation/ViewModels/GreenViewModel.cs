using System;
using Jounce.Core.ViewModel;
using Jounce.Framework;

namespace SimpleNavigation.ViewModels
{
    [ExportAsViewModel("GreenCircle")]
    public class GreenViewModel : BaseViewModel 
    {
        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged(()=>Text);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            Text = viewParameters.ParameterValue<Guid>("Guid").ToString();
            base.ActivateView(viewName, viewParameters);
        }
    }   
}