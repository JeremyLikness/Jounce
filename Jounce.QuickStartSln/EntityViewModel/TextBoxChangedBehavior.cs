using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EntityViewModel
{
    /// <summary>
    ///     Behavior to force a text box to instantly update its binding
    /// </summary>
    public class TextBoxChangedBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += AssociatedObject_TextChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObject_TextChanged;
        }

        static void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;
            var binding = tb.GetBindingExpression(TextBox.TextProperty);
            if (binding != null)
            {
                binding.UpdateSource();
            }
        }
    }
}