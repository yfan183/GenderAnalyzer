using GenderAnalyzer.DataModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GenderAnalyzer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AzureTable : ContentPage
    {
        public AzureTable()
        {
            MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

            InitializeComponent();
            BindingContext = new AzureTablesViewModel();
        }

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<GenderGuesserModel> genderInformation = await AzureManager.AzureManagerInstance.getGenderInfo();
            foreach (var i in genderInformation)
            {
                
            }
            GenderList.ItemsSource = genderInformation;
        }
    }

    class AzureTablesViewModel : INotifyPropertyChanged
    {

        public AzureTablesViewModel()
        {
            IncreaseCountCommand = new Command(IncreaseCount);
        }

        int count;

        string countDisplay = "You clicked 0 times.";
        public string CountDisplay
        {
            get { return countDisplay; }
            set { countDisplay = value; OnPropertyChanged(); }
        }

        public ICommand IncreaseCountCommand { get; }

        void IncreaseCount() =>
            CountDisplay = $"You clicked {++count} times";


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}