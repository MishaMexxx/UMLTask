using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using BaseFrame;

namespace DBClient
{
	public class SelectionBehavior : Behavior<DataGrid>
	{
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof(ICommand), typeof(SelectionBehavior), new PropertyMetadata(default(string)));

		public static readonly DependencyProperty CurrentProperty =
		DependencyProperty.Register("CurrentData", typeof(ViewModelBase), typeof(SelectionBehavior), new PropertyMetadata(default(string)));

		public ICommand Command
		{
			get => (ICommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public ViewModelBase CurrentData
		{
			get => (ViewModelBase) GetValue(CurrentProperty);
			set => SetValue(CommandProperty, value);
		}

		protected override void OnAttached()
		{
			AssociatedObject.SelectionChanged += this.AssociatedObject_SelectionChanged;
		}

		private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Command?.Execute(CurrentData);
		}

		protected override void OnDetaching()
		{
			AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
		}

	}
}
