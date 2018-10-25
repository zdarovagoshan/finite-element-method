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
			DataContext = new ViewModels.MainWindowViewModel();
			/* var kek = this.PlatformImpl?.Handle;
			this.PlatformImpl?.Screen;
			this.Window; */
#if DEBUG
            this.AttachDevTools();
#endif
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
			//this.PlatformImpl?.Handle;
		}
	}
}
/* public class DllImportFunctions
{
    [DllImport("glx")]
    public static extern int main();

} */
