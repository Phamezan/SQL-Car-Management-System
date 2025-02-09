using Første_SQL.ViewModels;
using System.Windows;

namespace Første_SQL.Views
{

    public partial class ManageUserWindow : Window
    {
        public ManageUserWindow()
        {
            InitializeComponent();
            DataContext = new ManageUserViewModel();
        }
    }
}
