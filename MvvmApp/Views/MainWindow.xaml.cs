using MvvmApp.Services;
using MvvmApp.ViewModels;
using MyImageLib;

namespace MvvmApp.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new FileDialogService(), new ImageProcessing());
        }
    }
}
