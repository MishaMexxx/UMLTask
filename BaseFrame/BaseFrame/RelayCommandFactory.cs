using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BaseFrame
{
	public class RelayCommandFactory : ICommandFactory
	{
		public ICommand CreateCommand(Action execute)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}

			return new RelayCommand<object>(o => execute());
		}
		public ICommand CreateCommand(Action execute, Func<bool> validate)
		{
			if (execute == null)
			{
				throw new ArgumentNullException("execute");
			}
			if (validate == null)
			{
				throw new ArgumentNullException("validate");
			}

			return new RelayCommand<object>(o => execute(), o => validate());
		}

		public ICommand CreateCommand(Action<object> execute)
		{
			return new RelayCommand<object>(execute);
		}
		public ICommand CreateCommand(Action<object> execute, Func<object, bool> validate)
		{
			if (validate == null)
			{
				throw new ArgumentNullException("validate");
			}

			return new RelayCommand<object>(execute, validate);
		}

		public ICommand CreateCommand<T>(Action<T> execute)
		{
			return new RelayCommand<T>(execute);
		}
		public ICommand CreateCommand<T>(Action<T> execute, Func<T, bool> validate)
		{
			if (validate == null)
			{
				throw new ArgumentNullException("validate");
			}

			return new RelayCommand<T>(execute, validate);
		}
	}
}
