using HolidayCountdown.MainMenuPage.NavPages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HolidayCountdown.MainMenuPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }

        private async void AddCountdown_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddMenu());
            Navigation.RemovePage(this);
        }

        private async void ModifyCountdown_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ModifyMenu());
            Navigation.RemovePage(this);
        }

        private async void RemoveCountdown_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RemoveMenu());
            Navigation.RemovePage(this);
        }
    }
}