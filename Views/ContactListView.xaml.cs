using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ContactManager.Model;
using ContactManager.Presenters;

namespace ContactManager.Views
{
    /// <summary>
    /// ContactListView.xaml 的交互逻辑
    /// </summary>
    public partial class ContactListView : UserControl
    {
        public ContactListView()
        {
            InitializeComponent();
        }

        public ContactListPresenter Presenter
        {
            get { return DataContext as ContactListPresenter; }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Presenter.Close();
        }

        private void OpenContact_Click(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            if (button != null)
                Presenter.OpenContact(button.DataContext as Contact);
        }
    }
}
