using Første_SQL.ViewModels.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Første_SQL.ViewModels
{
    public class NavigationService : INavigationService
    {

        public void OpenWindow<T>() where T : Window, new()
        {    
            var window = new T();
            window.Show();
        }
        public void CloseWindow<T>() where T : Window
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is T)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
