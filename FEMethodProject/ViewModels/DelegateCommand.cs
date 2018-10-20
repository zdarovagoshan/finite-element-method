using System;
using System.Windows.Input;


namespace FEMethodProject.ViewModels
{
	public class DelegateCommand : ICommand
	{
		#region ctor

		public DelegateCommand(Action<object> execute) : this(execute, null)
		{
			Name = "Default command name";
		}

		public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion

		#region Public properties
		public string Name { get; set; }
		#endregion

		#region ICommand implementation

		public event EventHandler CanExecuteChanged;
		

		public bool CanExecute(object commandParameter)
		{
			if (_canExecute != null)
				return _canExecute(commandParameter);

			return true;
		}

		public void Execute(object commandParamter)
		{
			if (_execute != null)
				_execute(commandParamter);
		}

		#endregion

		#region Private members
		private string _commandName;
		private Action<object> _execute;
		private Func<object, bool> _canExecute;
		#endregion
	}
}
