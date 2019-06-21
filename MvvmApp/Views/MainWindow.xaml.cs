using MvvmApp.Services;
using MvvmApp.ViewModels;
using MyImageLib;

namespace MvvmApp.Views
{
	/// <summary>
	/// Logika interakcji dla klasy MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();
            DataContext = new Presenter(new FileDialogService(),new ImageProcessing());
		}
	}
}
