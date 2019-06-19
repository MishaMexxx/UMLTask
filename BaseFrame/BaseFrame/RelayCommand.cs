using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BaseFrame
{
	public class RelayCommand<TArgument> : ICommand 
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public RelayCommand(
			Action<TArgument> execute,
			Func<TArgument, bool> validate = null)
		{
			if (execute == null)
			{
				throw new ArgumentNullException();
			}

			_execute = execute;
			_validate = validate;
		}

		#region CanExecute
		public bool CanExecute(TArgument parameter)
		{
			return _validate == null || _validate(parameter);
		}

		bool ICommand.CanExecute(object parameter)
		{
			return this.CanExecute((TArgument)parameter);
		}
		#endregion

		#region Execute
		public void Execute(TArgument parameter)
		{
			_execute(parameter);
		}

		void ICommand.Execute(object parameter)
		{
			this.Execute((TArgument)parameter);
		}
		#endregion

		private readonly Action<TArgument> _execute;
		private readonly Func<TArgument, bool> _validate;
	}
}
