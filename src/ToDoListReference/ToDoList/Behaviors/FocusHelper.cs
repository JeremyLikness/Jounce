using System.Windows;
using System.Windows.Controls;

namespace ToDoList.Behaviors
{
    /// <summary>
    /// Helper control that will set the focus to an associated control whenever the
    /// <seealso cref="SetFocus"/> property is set to true
    /// </summary>
    /// <remarks>
    /// This makes it possible to set focus using a <seealso cref="VisualState"/>   
    /// </remarks>
    public class FocusHelper : Control
    {
        /// <summary>
        /// Element to set focus on
        /// </summary>
        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(
                "TargetElement",
                typeof (Control),
                typeof (FocusHelper),
                null);

        /// <summary>
        /// Set this to true to trigger the focus
        /// </summary>
        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.Register(
                "SetFocus",
                typeof(bool),
                typeof(FocusHelper),
                new PropertyMetadata(false, FocusChanged));
        
        /// <summary>
        /// Element to set focus on
        /// </summary>
        public Control TargetElement
        {
            get { return (Control) GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }            
        }

        /// <summary>
        /// Set this to true to trigger the focus
        /// </summary>
        public bool SetFocus
        {
            get { return (bool) GetValue(SetFocusProperty); }
            set { SetValue(SetFocusProperty, value);}
        }

        /// <summary>
        /// This is called whenever the <seealso cref="SetFocus"/> property changes
        /// </summary>
        /// <param name="d">The <see cref="FocusHelper"/> control that fired the event</param>
        /// <param name="e">The changes to the <seealso cref="SetFocus"/> property</param>
        private static void FocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var targetElement = d.GetValue(TargetElementProperty) as Control;
            if (targetElement == null || e.NewValue == null || (!((bool) e.NewValue)))
            {
                return;
            }
            targetElement.Focus();
            d.SetValue(SetFocusProperty, false);
        }
    }
}