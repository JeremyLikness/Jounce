using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;

namespace SimpleNavigationWithRegion.Views
{
    /// <summary>
    ///     Binds the view model to all of the views
    /// </summary>
    public class Bindings
    {
        [Export]
        public ViewModelRoute Circle
        {
            get { return ViewModelRoute.Create("ShapeViewModel", "GreenCircle"); }
        }

        [Export]
        public ViewModelRoute Square
        {
            get { return ViewModelRoute.Create("ShapeViewModel", "RedSquare"); }
        }

        [Export]
        public ViewModelRoute Text
        {
            get { return ViewModelRoute.Create("ShapeViewModel", "TextView"); }
        }
    }
}