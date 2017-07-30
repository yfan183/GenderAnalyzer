using GenderAnalyzer.DataModels;
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
    public partial class MyEntry : ContentPage
    {
        public MyEntry()
        {
            InitializeComponent();
            BindingContext = new MyEntryViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await postStatsAsync(LocationEntry.Text, MaleEntry.Text, FemaleEntry.Text);
            await DisplayAlert("Thanks", "We have updated our stats with your submission.", "Ok");
        }
        async Task postStatsAsync(string location, string malepop, string femalepop)
        {
            GenderGuesserModel genderModel = new GenderGuesserModel()
            {
                Location = location,
                Male = malepop,
                Female = femalepop

            };

            await AzureManager.AzureManagerInstance.PostGenderStats(genderModel);
        }
    }

    class MyEntryViewModel : INotifyPropertyChanged
    {
        
        public MyEntryViewModel()
        {
            IncreaseCountCommand = new Command(IncreaseCount);
        }

        int count;

        string countDisplay = "You clicked 0 times.";
        public string CountDisplay
        {
            get { return countDisplay; }
            set { countDisplay = value;  OnPropertyChanged(); }
        }

        public ICommand IncreaseCountCommand { get; }

        void IncreaseCount() =>
            CountDisplay = $"You clicked {++count} times";


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
    }
}