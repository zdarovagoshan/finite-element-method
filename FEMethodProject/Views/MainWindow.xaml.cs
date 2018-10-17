using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FEMethodProject.Views
{
	public class MainWindow : Window
	{
		private Button _load;
        private Button _solve;
		public MainWindow()
		{
			InitializeComponent();
			_load.Click += delegate
            {
                //Действие при нажатии кнопки _load
            };
            _solve.Click += delegate
            {
                //Действие при нажатии кнопки solve
            };
#if DEBUG
            this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			_load = this.FindControl<Button>("load");
            _solve = this.FindControl<Button>("solve");
		}
	}
}
