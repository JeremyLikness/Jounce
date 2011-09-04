using System;

namespace Jounce.Core.View
{
    /// <summary>
    ///     Called after the view is navigated to
    /// </summary>
    public class ViewNavigatedArgs : ViewNavigationArgs
    {
        public ViewNavigatedArgs(Type viewType) : base(viewType)
        {
        }

        public ViewNavigatedArgs(string viewType) : base(viewType)
        {
        }
    }
}