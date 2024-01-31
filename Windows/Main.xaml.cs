using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using Lab4.Models;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private HttpClient httpClient;
        private MainWindow mainWindow;
        private string? token;

        public Main(Response response, MainWindow window)
        {
            InitializeComponent();
            this.mainWindow = window;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + response.access_token);
            token = response.access_token;
            Task.Run(Load);
        }
        private async Task Load()
        {
            List<Patient>? list = await httpClient.GetFromJsonAsync<List<Patient>>("http://localhost:18467/api/patients");
            foreach (Patient i in list!)
            {
                i.Tariff = await httpClient.GetFromJsonAsync<Models.Tariff>("http://localhost:18467/api/tariffs/" + i.TariffId);
            }
            Dispatcher.Invoke(() =>
            {
                ListPatients.ItemsSource = null;
                ListPatients.Items.Clear();
                ListPatients.ItemsSource = list;
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainWindow.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TariffWindow tariffWindow = new TariffWindow(token!);
            tariffWindow.ShowDialog();
        }

        // добавление
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PatientWindow patientWindow = new PatientWindow(token!);
            if (patientWindow.ShowDialog() == true)
            {
                Patient patient = new Patient
                {
                    FullName         = patientWindow.FullNameProperty,
                    Days             = patientWindow.DaysProperty,
                    PlaceOfResidence = patientWindow.PlaceOfResidenceProperty,
                    Gender           = patientWindow.GenderProperty,
                    Age              = patientWindow.AgeProperty,
                    DiagnosisName    = patientWindow.DiagnosisNameProperty,
                    DiagnosisCode    = patientWindow.DiagnosisCodeProperty,
                    TariffId         = await patientWindow.getTariffId(),
                    Cost             = patientWindow.CostProperty
                };
                JsonContent content = JsonContent.Create(patient);
                using var response = await httpClient.PostAsync("http://localhost:18467/api/patients", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }

        // изменение
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Patient? st = ListPatients.SelectedItem as Patient;
            PatientWindow patientWindow = new PatientWindow(token!, st!);
            if (patientWindow.ShowDialog() == true)
            {
                st!.FullName         = patientWindow.FullNameProperty;
                st!.Days             = patientWindow.DaysProperty;
                st!.PlaceOfResidence = patientWindow.PlaceOfResidenceProperty;
                st!.Gender           = patientWindow.GenderProperty;
                st!.Age              = patientWindow.AgeProperty;
                st!.DiagnosisName    = patientWindow.DiagnosisNameProperty;
                st!.DiagnosisCode    = patientWindow.DiagnosisCodeProperty;
                st!.TariffId         = await patientWindow.getTariffId();
                st!.Cost             = patientWindow.CostProperty;

                JsonContent content = JsonContent.Create(st);
                using var response = await httpClient.PutAsync("http://localhost:18467/api/patients", content);
                string responseText = await response.Content.ReadAsStringAsync();
                await Load();
            }
        }

        // удаление
        private async void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Patient? st = ListPatients.SelectedItem as Patient;
            JsonContent content = JsonContent.Create(st);
            using var response = await httpClient.DeleteAsync("http://localhost:18467/api/patients/" + st!.Id);
            string responseText = await response.Content.ReadAsStringAsync();
            await Load();
        }
    }
}
