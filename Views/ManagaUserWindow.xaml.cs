using Første_SQL.ViewModels;
using System.Windows;

namespace Første_SQL.Views
{

    public partial class ManagaUserWindow : Window
    {
        public ManagaUserWindow()
        {
            InitializeComponent();
            DataContext = new ManageUserViewModel();
        }
    }
}
