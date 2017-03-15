using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Controls;

using ContactManager.Presenters;


namespace ContactManager.UserControls
{
    /// <summary>
    /// SearchBar.xaml 的交互逻辑
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public SearchBar()
        {
            InitializeComponent();
        }

        public ApplicationPresenter Presenter
        {
            get { return DataContext as ApplicationPresenter; }
        }

        private void SearchText_Changed(object sender, TextChangedEventArgs e)
        {
            Presenter.Search(searchText.Text);
        }
    }
}
