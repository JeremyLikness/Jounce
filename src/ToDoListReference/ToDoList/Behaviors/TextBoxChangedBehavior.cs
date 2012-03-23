using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ToDoList.Behaviors
{
    /// <summary>
    /// Behavior that causes a text box to update changes through data-binding
    /// as the user types rather than the default of only updating when the
    /// text box loses focus
    /// </summary>
    public class TextBoxChangedBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Called when the behavior is attached to the <seealso cref="TextBox"/>
        /// </summary>
        protected override void OnAttached()
        {
            AssociatedObject.TextChanged += AssociatedObjectTextChanged;
        }

        /// <summary>
        /// Fired when the text has changed in the target <seealso cref="TextBox"/>
        /// </summary>
        /// <param name="sender">The <seealso cref="TextBox"/> that fired the change</param>
        /// <param name="e">The <seealso cref="TextChangedEventArgs"/> for the change</param>
        void AssociatedObjectTextChanged(object sender, TextChangedEventArgs e)
        {
            // grab the binding 
            var binding = AssociatedObject.GetBindingExpression(TextBox.TextProperty);

            // if it exists, update it 
            if (binding != null)
            {
                binding.UpdateSource();
            }

            // defer to base
            base.OnAttached();
        }

        /// <summary>
        /// Called when the behavior is detached. Use to unhook event handlers.
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.TextChanged -= AssociatedObjectTextChanged;
            base.OnDetaching();
        }
    }
}