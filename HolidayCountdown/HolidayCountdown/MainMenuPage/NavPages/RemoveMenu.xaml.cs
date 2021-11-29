using HolidayCountdown.CountdownObj;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HolidayCountdown.MainMenuPage.NavPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RemoveMenu : ContentPage
    {
        /// Json
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.json");
        private string json = "";

        /// Constructor
        public RemoveMenu()
        {
            InitializeComponent();
            OnLoaded();
        }

        /// Go Back To MenuPage
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
            Navigation.RemovePage(this);
        }

        /// On Click Remove The Countdown And Go Back To MenuPage
        private async void REMOVE_Clicked(object sender, EventArgs e)
        {
            List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);
            jsonData.Remove(jsonData[CountdownPick.SelectedIndex]);
            json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
            File.WriteAllText(fileName, json);

            await Navigation.PushAsync(new MainMenu());
            Navigation.RemovePage(this);
        }

        /// On Loaded Method Goes In Constructor
        private void OnLoaded()
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                json = r.ReadToEnd();
            }

            List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);
            CountdownPick.ItemsSource = jsonData;

        }
    }
}