using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodingDad.NET.Common.UserCreationLogin.Views
{
	public partial class UserLoginView : UserControl
	{
		public static readonly DependencyProperty ButtonColorProperty = DependencyProperty.Register(
			"ButtonColor", typeof(Brush), typeof(UserLoginView), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

		public static readonly DependencyProperty ControlBackgroundProperty = DependencyProperty.Register(
			"ControlBackground", typeof(Brush), typeof(UserLoginView), new PropertyMetadata(new SolidColorBrush(Colors.White)));

		public static readonly DependencyProperty ControlCornerRadiusProperty = DependencyProperty.Register(
			"ControlCornerRadius", typeof(CornerRadius), typeof(UserLoginView), new PropertyMetadata(new CornerRadius(5)));

		public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
			"PlaceholderText", typeof(string), typeof(UserLoginView), new PropertyMetadata("Username"));

		public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(
			"ShadowColor", typeof(Color), typeof(UserLoginView), new PropertyMetadata(Colors.Gray));

		public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register(
			"ShadowDepth", typeof(double), typeof(UserLoginView), new PropertyMetadata(2.0));

		public UserLoginView ()
		{
			InitializeComponent();
		}

		public Brush ButtonColor
		{
			get => (Brush)GetValue(ButtonColorProperty);
			set => SetValue(ButtonColorProperty, value);
		}

		public Brush ControlBackground
		{
			get => (Brush)GetValue(ControlBackgroundProperty);
			set => SetValue(ControlBackgroundProperty, value);
		}

		public CornerRadius ControlCornerRadius
		{
			get => (CornerRadius)GetValue(ControlCornerRadiusProperty);
			set => SetValue(ControlCornerRadiusProperty, value);
		}

		public string PlaceholderText
		{
			get => (string)GetValue(PlaceholderTextProperty);
			set => SetValue(PlaceholderTextProperty, value);
		}

		public Color ShadowColor
		{
			get => (Color)GetValue(ShadowColorProperty);
			set => SetValue(ShadowColorProperty, value);
		}

		public double ShadowDepth
		{
			get => (double)GetValue(ShadowDepthProperty);
			set => SetValue(ShadowDepthProperty, value);
		}

		private void HidePlaceholder (object sender, RoutedEventArgs e)
		{
			UsernamePlaceholder.Visibility = Visibility.Collapsed;
		}

		private void ShowPlaceholder (object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(UsernameTextBox.Text))
			{
				UsernamePlaceholder.Visibility = Visibility.Visible;
			}
		}
	}
}