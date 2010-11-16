using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EntityViewModel
{
    /// <summary>
    ///     Focus a text box based on the trigger
    /// </summary>
    public class FocusTrigger : TargetedTriggerAction<TextBox>
    {
        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            var tb = TargetObject as TextBox;
            if (tb != null)
            {
                tb.Focus();
            }
        }
    }
}