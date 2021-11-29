using HolidayCountdown.CountdownObj;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HolidayCountdown.MainMenuPage.NavPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMenu : ContentPage
    {
        /// Json
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.json");
        private string json = "";

        /// Image Path
        private string imagePath = "";

        /// Video Path
        private string videoPath = "";

        /// Constructor
        public AddMenu()
        {
            InitializeComponent();
        }

        /// Go Back To MenuPage
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
            Navigation.RemovePage(this);
        }

        /// Check If Name Are Selected
        private bool CheckIfNameEmpty()
        {
            if (NameCountDown.Text == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// Pick An Image For Background
        private async void PickImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pick = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please Select Your Background Photo"
                });

                imagePath = pick.FullPath;
                PickImage.BackgroundColor = Color.Green;
            }
            catch (Exception ex)
            {
                PickImage.BackgroundColor = Color.Red;
                await DisplayAlert("Error", "You didn't choose an Image, try again!", "OK");
            }
        }

        /// Pick A Video For Background
        private async void PickVideo_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pick = await MediaPicker.PickVideoAsync(new MediaPickerOptions
                {
                    Title = "Please Select Your Background Video"
                });

                videoPath = pick.FullPath;
                PickVideo.BackgroundColor = Color.Green;
            }
            catch (Exception ex)
            {
                PickVideo.BackgroundColor = Color.Red;
                await DisplayAlert("Error", "You didn't choose a Video, try again!", "OK");
            }
        }

        /// On Click Add The Countdown And Go Back To MenuPage
        private async void ADD_Clicked(object sender, EventArgs e)
        {
            if (CheckIfNameEmpty())
            {
                Countdown buttonObj = new Countdown()
                {
                    Name = NameCountDown.Text,
                    Date = DateCountDown.Date,
                    ImagePath = imagePath.Length == 0 ? "" : imagePath,
                    VideoPath = videoPath.Length == 0 ? "" : videoPath,
                };

                using (StreamReader r = new StreamReader(fileName))
                {
                    json = r.ReadToEnd();
                }

                List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);
                jsonData.Add(buttonObj);
                json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                File.WriteAllText(fileName, json);

                await Navigation.PushAsync(new MainMenu());
                Navigation.RemovePage(this);
            }
            else
            {
                await DisplayAlert("Error", "You have not set a name for the Countdown!", "OK");
            }
        }
    }
}