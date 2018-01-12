namespace Shared.ViewModelInterfaces
{
    public interface IMainWindowViewModel<T>
    {
        IListViewModel<T> ListViewModel { get; }
    }
}