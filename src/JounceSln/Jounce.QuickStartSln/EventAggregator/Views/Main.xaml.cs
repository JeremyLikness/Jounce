using System.Windows.Controls;
using Jounce.Core.View;

namespace EventAggregator.Views
{
    /// <summary>
    ///     Export the main view
    /// </summary>
    [ExportAsView(Constants.VIEW_MAIN,IsShell=true)]
    public partial class Main
    {
        public Main()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Because this is composed inside the main view, we export here to get 
        ///     the correct instance instead of on the class itself        
        /// </summary>
        [ExportAsView(Constants.VIEW_SENDER)]
        public UserControl SenderExport
        {
            get
            {
                return SenderView;
            }
        }

        /// <summary>
        ///     Same with the receiver
        /// </summary>
        [ExportAsView(Constants.VIEW_RECEIVER)]
        public UserControl ReceiverExport
        {
            get
            {
                return ReceiverView;
            }
        }
    }
}
