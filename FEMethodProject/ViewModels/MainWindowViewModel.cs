using System;
using ReactiveUI;
using Avalonia.Controls;
using System.Windows.Input;

namespace FEMethodProject.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public Window Window { get; set; }
		public MainWindowViewModel()
		{
			_title = "Finite Element Method";
			_isCalculating = _isGridReading = false;

			_loadGridCommand = new DelegateCommand(LoadGrid, CanLoadGrid) { Name = "Загрузить сетку" };
			_calculateCommand = new DelegateCommand(Calculate, CanCalculate) { Name = "Рассчитать решение" };
		}

		#region Public methods

		public async void LoadGrid(object parameter)
		{
			_isGridReading = true;

			OpenFileDialog d = new OpenFileDialog();
			d.Title = "Загрузка файла сетки";

			string[] selectedFiles = await d.ShowAsync(Window);
			if (selectedFiles != null) PathToGridFile = selectedFiles[0];

			// Code for mesh reading

			_isGridReading = false;
		}

		public void Calculate(object parameter)
		{
			_isCalculating = true;

			throw new Exception("Иди нахуй");
			// Code for call models and get FEM solution

			_isCalculating = false;
		}

		public bool CanLoadGrid(object parameter)
		{
			return !_isGridReading;
		}

		public bool CanCalculate(object parameter)
		{
			return !_isCalculating && !_isGridReading && !string.IsNullOrEmpty(PathToGridFile);
		}
		
		#endregion

		#region Private members
		private string _title;
		private bool _isGridReading;
		private bool _isCalculating;
		private string _pathToGridFile;

		// Commands
		private ICommand _loadGridCommand;
		private ICommand _calculateCommand;
		#endregion

		#region Public properties

		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(ref _title, value); }
		}

		public string PathToGridFile
		{
			get { return _pathToGridFile; }
			set { this.RaiseAndSetIfChanged(ref _pathToGridFile, value); }
		}

		#endregion

		#region ViewModel commands

		public ICommand LoadGridCommand
		{
			get { return _loadGridCommand; }
			private set { _loadGridCommand = value; }
		}
		public ICommand CalculateCommand
		{
			get { return _calculateCommand; }
			private set { _calculateCommand = value; }
		}

		#endregion
	}
}
