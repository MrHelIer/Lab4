using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Lab4.Models;

namespace Lab4
{
    /// <summary>
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window
    {
        private HttpClient client;
        private Patient? patient;
        public PatientWindow(String token)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(LoadTariffs);
        }
        public PatientWindow(string token, Patient patient)
        {
            InitializeComponent();
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            Task.Run(() => LoadTariffs());

            FullName.Text           = patient.FullName;
            Days.Text               = patient.Days.ToString();
            PlaceOfResidence.Text   = patient.PlaceOfResidence;
            cbGender.SelectedItem   = patient.Gender;
            Age.Text                = patient.Age.ToString();
            DiagnosisName.Text      = patient.DiagnosisName;
            DiagnosisCode.Text      = patient.DiagnosisCode;
            cbTariffs.SelectedItem  = patient.Tariff!.Name;
            Cost.Text               = patient.Cost.ToString("F2");
        }
        private async void LoadTariffs()
        {
            List<Tariff>? list = await client.GetFromJsonAsync<List<Tariff>>("http://localhost:18467/api/tariffs");
            Dispatcher.Invoke(() =>
            {
                cbTariffs.ItemsSource = list?.Select(p=>p.Name);
            });
        }
        public string? FullNameProperty
        {
            get { return FullName.Text; }
        }
        public int DaysProperty
        {
            get { return int.Parse(Days.Text); }
        }
        public string? PlaceOfResidenceProperty
        {
            get { return PlaceOfResidence.Text; }
        }
        public string? GenderProperty
        {
            get { return cbGender.Text; }
        }
        public int AgeProperty
        {
            get { return int.Parse(Age.Text); }
        }
        public string? DiagnosisNameProperty
        {
            get { return DiagnosisName.Text; }
        }
        public string? DiagnosisCodeProperty
        {
            get { return DiagnosisCode.Text; }
        }
        public double CostProperty
        {
            get { return double.Parse(Cost.Text); }
        }
        public async Task<int> getTariffId()
        {
            Tariff? tariff = (await client.GetFromJsonAsync<List<Tariff>>("http://localhost:18467/api/tariffs/" + cbTariffs.Text))?.First();
            return tariff!.Id;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }
    }
}
