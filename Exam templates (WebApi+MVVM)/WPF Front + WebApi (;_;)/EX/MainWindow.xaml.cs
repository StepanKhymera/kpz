using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
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
using Nancy.Json;
using System.Collections.ObjectModel;

namespace EX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Song> playlist;

        public MainWindow()
        {
            InitializeComponent();

            playlist = new ObservableCollection<Song>();
            plst.ItemsSource = playlist;
            
            this.Dispatcher.Invoke(async () =>
            {
                gr.ItemsSource = await WebAPIGetList("http://localhost:5000/api/Songs");
            });
        }
        static HttpClient client = new HttpClient();

        private async void gr_RowEditEndingAsync(object sender, DataGridRowEditEndingEventArgs e)
        {

            if (this.gr.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= gr_RowEditEndingAsync;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).RowEditEnding += gr_RowEditEndingAsync;
            }
            else return;
            Song edit = (Song)this.gr.SelectedItem;


            List<Song> project = await WebAPIGetList("http://localhost:5000/api/Songs");
            WebAPIPut( $"http://localhost:5000/api/Songs/post/{edit.SongID}", edit);

        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = await client.PostAsync("http://localhost:5000/api/Songs", new StringContent(new JavaScriptSerializer().Serialize(new Song()), Encoding.UTF8, "application/json"));

            //rep.Add(new Song());
            gr.ItemsSource = await WebAPIGetList("http://localhost:5000/api/Songs");
        }

        private async void gr_MouseDoubleClickAsync(object sender, MouseButtonEventArgs e)
        {
            HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5000/api/Songs/delete/{((Song)gr.SelectedItem).SongID}");
            //rep.Delete(((Song)gr.SelectedItem).SongID);
            gr.ItemsSource = await WebAPIGetList("http://localhost:5000/api/Songs");
        }
        static async Task<List<Song>> WebAPIGetList(string path)
        {
            List<Song> project = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                project = await response.Content.ReadAsAsync <List<Song>>();
            }
            return project;
        }
        static async Task<Song> WebAPIGet(string path)
        {
            Song project = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                project = await response.Content.ReadAsAsync<Song>();
            }
            return project;
        }
        static async Task  WebAPIPut(string path, Song body)
        {

            var Content = new StringContent(new JavaScriptSerializer().Serialize(body), Encoding.UTF8, "application/json");
            try
            {
                HttpResponseMessage response = await client.PostAsync(path, Content);
            }
            catch (Exception ex)
            {
                throw;
            }
            return; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!playlist.Contains((Song)(((System.Windows.Controls.Button)sender).DataContext))) return;
            playlist.Remove((Song)(((System.Windows.Controls.Button)sender).DataContext));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (playlist.Contains((Song)(((System.Windows.Controls.Button)sender).DataContext))) return;
            playlist.Add((Song)(((System.Windows.Controls.Button)sender).DataContext));
        }
    }
}
