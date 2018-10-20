using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FEMethodProject.Views
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DataContext = new ViewModels.MainWindowViewModel() { Window = this };
#if DEBUG
            this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
