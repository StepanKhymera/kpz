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

namespace databaseF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPolicyRepository rep;

        public MainWindow()
        {
            InitializeComponent();
            rep = GetEntityFrameworkRepository();
            gr.ItemsSource = rep.GetAll();
        }

        private IPolicyRepository GetEntityFrameworkRepository()
        {
            var repo = new PolicyRepository();

            return repo;
        }

        private IPolicyRepository GetAdoRepository()
        {
            var repo = new PolicyRepositoryAdo();

            return repo;
        }

        private void gr_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            if (this.gr.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= gr_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += gr_RowEditEnding;
            }
            else return;
            Policy edit = (Policy)this.gr.SelectedItem;
            rep.Update(edit);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rep.Add(new Policy());
            gr.ItemsSource = rep.GetAll();
        }

        private void gr_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rep.Delete(((Policy)gr.SelectedItem).PolicyID);
            gr.ItemsSource = rep.GetAll();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rep.SaveChanges();
        }

        private void gr_RowEditEnding_1(object sender, DataGridRowEditEndingEventArgs e)
        {

        }
    }
}
