using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DBClient
{
	public class StadiumsDataGridBehavior : Behavior<DataGrid>
	{
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(StadiumsDataGridBehavior), new PropertyMetadata(default(string)));

		public static readonly DependencyProperty MatchProperty =
		DependencyProperty.Register("Stadium", typeof(StadiumInfo), typeof(StadiumsDataGridBehavior), new PropertyMetadata(default(string)));

		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public StadiumInfo Stadium
		{
			get => (StadiumInfo) GetValue(MatchProperty);
			set => SetValue(CommandProperty, value);
		}

		protected override void OnAttached()
		{
			AssociatedObject.SelectionChanged += this.AssociatedObject_SelectionChanged;
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Command?.Execute(Stadium);
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
		}

	}
}
