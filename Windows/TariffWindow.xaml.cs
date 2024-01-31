using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab4.Models;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для GroupWindow.xaml
    /// </summary>
    public partial class TariffWindow : Window
    {
        private HttpClient client;
        private Tariff? tariff;
        public TariffWindow(string token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(Load);
        }
        private async Task Load()
        {
            List<Tariff>? list = await client.GetFromJsonAsync<List<Tariff>>("http://localhost:18467/api/tariffs");
            Dispatcher.Invoke(() =>
            {
                ListAssortiments.ItemsSource = null;
                ListAssortiments.Items.Clear();
                ListAssortiments.ItemsSource = list;
            });
        }
        private async Task Save()
        {
            Tariff tariff = new Tariff
            {
                Name          = TariffName.Text,
                DiagnosisCode = DiagnosisCode.Text,
                DiagnosisName = DiagnosisName.Text,
                Cost          = double.Parse(Price.Text)
            };
            JsonContent content = JsonContent.Create(tariff);
            using var response = await client.PostAsync("http://localhost:18467/api/tariffs", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Save();
        }

        private void ListTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tariff = ListAssortiments.SelectedItem as Tariff;
            TariffName.Text = tariff?.Name;
            DiagnosisName.Text = tariff?.DiagnosisName;
            DiagnosisCode.Text = tariff?.DiagnosisCode;
            Price.Text = tariff?.Cost.ToString("F2");
        }

        private async Task Edit()
        {
            tariff!.Name = TariffName.Text;
            tariff!.DiagnosisCode = DiagnosisCode.Text;
            tariff!.DiagnosisName = DiagnosisName.Text;
            tariff!.Cost = double.Parse(Price.Text);

            JsonContent content = JsonContent.Create(tariff);
            using var response = await client.PutAsync("http://localhost:18467/api/tariffs", content);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async Task Delete()
        {
            using var response = await client.DeleteAsync("http://localhost:18467/api/tariffs/" + tariff?.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            await Delete();
        }
    }
}
