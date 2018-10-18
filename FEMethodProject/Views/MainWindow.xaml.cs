using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FEMethodProject.Views
{
	public class MainWindow : Window
	{
		private Button _load;
        private Button _solve;
		private ConsoleApplication1.mke m;
		
		public MainWindow()
		{
			InitializeComponent();
			_load.Click += delegate
            {
                //Действие при нажатии кнопки _load
	            m = new ConsoleApplication1.mke();
            };
            _solve.Click += delegate
            {
                //Действие при нажатии кнопки solve
	            m.solution();
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
