using System;
using System.Windows;
using System.Windows.Input;

namespace CodingDad.Common.Behaviors
{
	public static class WindowCloseBehavior
	{
		public static readonly DependencyProperty CloseCommandProperty =
			DependencyProperty.RegisterAttached("CloseCommand", typeof(ICommand), typeof(WindowCloseBehavior), new PropertyMetadata(null, OnCloseCommandChanged));

		public static ICommand GetCloseCommand (DependencyObject obj) => (ICommand)obj.GetValue(CloseCommandProperty);

		public static void SetCloseCommand (DependencyObject obj, ICommand value) => obj.SetValue(CloseCommandProperty, value);

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