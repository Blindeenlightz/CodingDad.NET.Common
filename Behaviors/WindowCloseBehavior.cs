using System;
using System.Windows;
using System.Windows.Input;

namespace CodingDad.NET.Common.Behaviors
{
    /// <summary>
    /// Provides attached properties to bind a command to the closing of a window.
    /// </summary>
    public static class WindowCloseBehavior
    {
        /// <summary>
        /// Identifies the CloseCommand attached property.
        /// </summary>
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(WindowCloseBehavior), new PropertyMetadata(null, OnCloseCommandChanged));

        /// <summary>
        /// Gets the CloseCommand property value from the specified element.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The ICommand value.</returns>
        public static ICommand GetCloseCommand (DependencyObject obj) => (ICommand)obj.GetValue(CloseCommandProperty);

        /// <summary>
        /// Sets the CloseCommand property value on the specified element.
        /// </summary>
        /// <param name="obj">The element on which to set the property value.</param>
        /// <param name="value">The ICommand value to set.</param>
        public static void SetCloseCommand (DependencyObject obj, ICommand value) => obj.SetValue(CloseCommandProperty, value);

        /// <summary>
        /// Callback method that gets invoked when the CloseCommand property changes.
        /// </summary>
        /// <param name="d">The element on which the property changed.</param>
        /// <param name="e">The event data.</param>
        private static void OnCloseCommandChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                window.Closed -= OnWindowClosed;
                if (e.NewValue != null)
                {
                    window.Closed += OnWindowClosed;
                }
            }
        }

        /// <summary>
        /// Handles the Closed event of the window and executes the bound command.
        /// </summary>
        /// <param name="sender">The window that is being closed.</param>
        /// <param name="e">The event data.</param>
        private static void OnWindowClosed (object sender, EventArgs e)
        {
            if (sender is Window window)
            {
                var command = GetCloseCommand(window);
                command?.Execute(null);
            }
        }
    }
}
