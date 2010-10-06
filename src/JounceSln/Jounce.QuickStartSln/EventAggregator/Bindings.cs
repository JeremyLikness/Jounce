using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;

namespace EventAggregator
{
    /// <summary>
    ///     Export all bindings
    /// </summary>
    public class Bindings
    {
        [Export]
        public ViewModelRoute MainBinding
        {
            get
            {
                return ViewModelRoute.Create(Constants.VM_MAIN, Constants.VIEW_MAIN);
            }
        }

        [Export]
        public ViewModelRoute SenderBinding
        {
            get
            {
                return ViewModelRoute.Create(Constants.VM_SENDER, Constants.VIEW_SENDER);
            }
        }

        [Export]
        public ViewModelRoute ReceiverBinding
        {
            get
            {
                return ViewModelRoute.Create(Constants.VM_RECEIVER, Constants.VM_RECEIVER);
            }
        }
    }
}