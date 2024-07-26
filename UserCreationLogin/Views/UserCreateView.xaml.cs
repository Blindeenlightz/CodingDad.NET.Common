using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CodingDad.Common.UserCreationLogin.Views
{
	/// <summary>
	/// Interaction logic for UserCreateView.xaml
	/// </summary>
	public partial class UserCreateView : UserControl
	{
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
			"Background", typeof(Brush), typeof(UserCreateView), new PropertyMetadata(new SolidColorBrush(Colors.White)));

		public static readonly DependencyProperty ButtonColorProperty = DependencyProperty.Register(
			"ButtonColor", typeof(Brush), typeof(UserCreateView), new PropertyMetadata(new SolidColorBrush(Colors.Blue)));

		public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
			"CornerRadius", typeof(CornerRadius), typeof(UserCreateView), new PropertyMetadata(new CornerRadius(5)));

		public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
									"PlaceholderText", typeof(string), typeof(UserCreateView), new PropertyMetadata("Username"));

		public static readonly DependencyProperty ShadowColorProperty = DependencyProperty.Register(
			"ShadowColor", typeof(Color), typeof(UserCreateView), new PropertyMetadata(Colors.Gray));

		public static readonly DependencyProperty ShadowDepthProperty = DependencyProperty.Register(
					"ShadowDepth", typeof(double), typeof(UserCreateView), new PropertyMetadata(2.0));

		public UserCreateView ()
		{
			InitializeComponent();
		}

		public Brush Background
		{
			get => (Brush)GetValue(BackgroundProperty);
			set => SetValue(BackgroundProperty, value);
		}

		public Brush ButtonColor
		{
			get { return (Brush)GetValue(ButtonColorProperty); }
			set { SetValue(ButtonColorProperty, value); }
		}

		public CornerRadius CornerRadius
		{
			get => (CornerRadius)GetValue(CornerRadiusProperty);
			set => SetValue(CornerRadiusProperty, value);
		}

		public string PlaceholderText
		{
			get { return (string)GetValue(PlaceholderTextProperty); }
			set { SetValue(PlaceholderTextProperty, value); }
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
			if (string.IsNullOrEmpty(NewUsernameTextBox.Text))
			{
				UsernamePlaceholder.Visibility = Visibility.Visible;
			}
		}
	}
}