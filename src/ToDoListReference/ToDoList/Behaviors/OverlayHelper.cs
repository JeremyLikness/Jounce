using System.Windows;

namespace ToDoList.Behaviors
{
    /// <summary>
    /// Simple class to toggle the visibility of an overlay layer
    /// </summary>
    public static class OverlayHelper
    {        
        /// <summary>
        /// Set this to the element that represents the overlay
        /// </summary>
        public static UIElement Overlay { get; set; }

        /// <summary>
        /// Show the overlay
        /// </summary>
        public static void Show()
        {
            if (Overlay != null)
            {
                Overlay.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hide the overlay
        /// </summary>
        public static void Hide()
        {
            if (Overlay != null)
            {
                Overlay.Visibility = Visibility.Collapsed;
            }
        }
    }
}