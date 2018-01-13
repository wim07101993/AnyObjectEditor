using TypelessDatabaseManager.ViewModels;

namespace TypelessDatabaseManager.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
