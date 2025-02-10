using System.Windows;

namespace Første_SQL.ViewModels.interfaces
{
    public interface INavigationService
    {
        void OpenWindow<T>() where T : Window, new();
        void CloseWindow<T>() where T : Window;

    }
}
