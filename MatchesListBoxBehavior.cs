using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DBClient
{
	public class MatchesListBoxItemBehavior : Behavior<Grid>
	{
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(MatchesListBoxItemBehavior), new PropertyMetadata(default(string)));

		public static readonly DependencyProperty MatchProperty =
		DependencyProperty.Register("Match", typeof(MatchInfo), typeof(MatchesListBoxItemBehavior), new PropertyMetadata(default(string)));

		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public MatchInfo Match
		{
			get => (MatchInfo) GetValue(MatchProperty);
			set => SetValue(CommandProperty, value);
		}

		protected override void OnAttached()
		{
			AssociatedObject.MouseLeftButtonDown += this.AssociatedObject_MouseLeftButtonDown;
			AssociatedObject.PreviewMouseRightButtonDown += this.AssociatedObject_MouseLeftButtonDown;
		}

		private void AssociatedObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			Command?.Execute(Match);
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseRightButtonDown -= this.AssociatedObject_MouseLeftButtonDown;
			AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
		}

	}
}