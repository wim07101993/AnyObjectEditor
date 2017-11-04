using DatabaseManager.ViewModelInterfaces;

namespace DatabaseManager.ViewModels
{
    public class MainWindowViewModel : IMainWindowVM 
    {
        public string Title { get; } = "Database Manager";
    }
}
