using System;
using System.ComponentModel.Composition;
using System.Windows;
using ToDoList.Contracts;
using ToDoList.Views;

namespace ToDoList.ViewModels
{
    [Export(typeof (INotification))]
    public class NotificationController : INotification
    {
        private NotificationWindow _window;

        public void Notify(string caption,
                            string text, TimeSpan time)
        {
            if (_window == null)
            {
                _window =
                    new NotificationWindow
                        {
                            Width = 302,
                            Height = 100
                        };
            }
            var control = new Notification
                                {
                                    NotificationTitle =
                                        {Text = caption},
                                    NotificationMessage =
                                        {Text = text}
                                };
            if (_window.Visibility == Visibility.Visible)
            {
                _window.Close();
            }
            _window.Content = control;
            _window.Show((int) time.TotalMilliseconds);
        }
    }
}