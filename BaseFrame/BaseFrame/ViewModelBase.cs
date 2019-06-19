using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
//using System.Windows.Controls;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BaseFrame
{
	public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo, IChangingInfo
	{
		public bool HasErrors => _validationErrors.Count > 0;
		public IEnumerable GetErrors(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName) || !_validationErrors.ContainsKey(propertyName))
			{
				return null;
			}

			return _validationErrors[propertyName];
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

		protected virtual void UpdateValue<TValue>(TValue value, ref TValue storage, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<TValue>.Default.Equals(value, storage))
			{
				return;
			}

			storage = value;
			if (propertyName != null)
			{
				this.RaisePropertyChanged(propertyName);
			}
		}
		protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new ArgumentNullException();
			}
			if (PropertyChanged != null)
			{
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		protected virtual void UpdateWithValidateValue<TValue>(TValue value, ref TValue storage, [CallerMemberName] string propertyName = null)
		{
			this.UpdateValue(value, ref storage, propertyName);
			this.ValidateModelProperty(value, propertyName);
		}

		private void RaiseErrorsChanged(string propertyName)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
		}
		private void ValidateModelProperty(object value, string propertyName)
		{
			if (_validationErrors.ContainsKey(propertyName))
			{
				_validationErrors.Remove(propertyName);
			}

			ICollection<ValidationResult> validationResults = new List<ValidationResult>();
			ValidationContext validationContext =
				new ValidationContext(this, null, null) { MemberName = propertyName };
			if (!Validator.TryValidateProperty(value, validationContext, validationResults))
			{
				_validationErrors.Add(propertyName, new List<string>());
				foreach (ValidationResult validationResult in validationResults)
				{
					_validationErrors[propertyName].Add(validationResult.ErrorMessage);
				}
			}
			RaiseErrorsChanged(propertyName);
		}
		protected void ClearValidated()
		{
			_validationErrors.Clear();
		}
		protected void ValidateModel<TViewModel>(TViewModel viewModel) where TViewModel : ViewModelBase
		{
			_validationErrors.Clear();
			ICollection<ValidationResult> validationResults = new List<ValidationResult>();
			ValidationContext validationContext = new ValidationContext(this, null, null);
			if (!Validator.TryValidateObject(this, validationContext, validationResults, true))
			{
				foreach (ValidationResult validationResult in validationResults)
				{
					string property = validationResult.MemberNames.ElementAt(0);
					if (_validationErrors.ContainsKey(property))
					{
						_validationErrors[property].Add(validationResult.ErrorMessage);
					}
					else
					{
						_validationErrors.Add(property, new List<string> { validationResult.ErrorMessage });
					}
				}

				var props = viewModel.GetType().GetProperties();
				foreach(var property in props)
				{
					this.RaiseErrorsChanged(property.Name);
				}
			}			
		}

		public void ValidateModel() => ValidateModel(this);

		#region IChangingInfo
		public bool HasChanged => this.HasChangedProperties();

		public void StartWatchingToChanging()
		{
			if (_isInitObservable)
			{
				return;
			}

			_isInitObservable = true;
			this.PropertyChanged += this.ObservableToChange;
		}
		public void StopWatchingToChanging()
		{
			if (!_isInitObservable)
			{
				return;
			}

			_isInitObservable = false;
			this.PropertyChanged -= this.ObservableToChange;
			_propertyChanged.Clear();
		}
		public void ResetWatchingToChanging()
		{
			this.StopWatchingToChanging();
			this.StartWatchingToChanging();
		}

		private void ObservableToChange(object o, PropertyChangedEventArgs a)
			=> _propertyChanged.Add(a.PropertyName);

		private bool HasChangedProperties()
		{
			var props = this.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

			return props.Any(prop => _propertyChanged.Contains(prop.Name));
		}

		private bool _isInitObservable = false;
		private List<string> _propertyChanged = new List<string>();
		#endregion

		private readonly Dictionary<string, ICollection<string>>
					_validationErrors = new Dictionary<string, ICollection<string>>();
	}
}

