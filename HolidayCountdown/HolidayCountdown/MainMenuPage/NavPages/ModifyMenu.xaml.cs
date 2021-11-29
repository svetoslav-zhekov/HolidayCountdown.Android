using HolidayCountdown.CountdownObj;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HolidayCountdown.MainMenuPage.NavPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyMenu : ContentPage
    {
        /// Json
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.json");
        private string json = "";

        /// Image/Video Path
        private string selectedPath = "";

        public ModifyMenu()
        {
            InitializeComponent();
            OnLoadedAsync();
        }

        /// Go Back To MenuPage
        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
            Navigation.RemovePage(this);
        }
    
        /// Display Or Don't Display Different Property Picker
        private void PropertyPickDisplay(bool name, bool date, bool image, bool video)
        {
            NameCD.IsVisible = name;
            DateCD.IsVisible = date;
            PickImage.IsVisible = image;
            PickVideo.IsVisible = video;
        }

        /// If PropertyPick Index Is Changed, Display Different Property Picker Depending On Said Index
        private void PropertyPick_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (PropertyPick.SelectedIndex)
            {
                case 0:
                    PropertyPickDisplay(true, false, false, false);
                    break;
                case 1:
                    PropertyPickDisplay(false, true, false, false);
                    break;
                case 2:
                    PropertyPickDisplay(false, false, true, false);
                    break;
                case 3:
                    PropertyPickDisplay(false, false, false, true);
                    break;
                default:
                    break;
            }
        }

        /// Pick A Image For Background
        private async void PickImage_Clicked(object sender, EventArgs e)
        {
            var pick = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Please Select Your Background Photo"
            });

            selectedPath = pick.FullPath;
            PickImage.BackgroundColor = Color.Green;
        }

        /// Pick A Video For Background
        private async void PickVideo_Clicked(object sender, EventArgs e)
        {
            var pick = await MediaPicker.PickVideoAsync(new MediaPickerOptions
            {
                Title = "Please Select Your Background Video"
            });

            selectedPath = pick.FullPath;
            PickVideo.BackgroundColor = Color.Green;
        }

        /// Pick A Countdown First Before A Property Check
        private async void PropertyPick_Focused(object sender, FocusEventArgs e)
        {
            if (CountdownPick.SelectedIndex == -1)
            {
                PropertyPick.Unfocus();
                await DisplayAlert("Error", "No Countdown picked, pick one first before proceeding to Property pick!", "OK");
                CountdownPick.Focus();
            }
        }

        /// On Click Modify The Countdown And Go Back To MenuPage
        private async void MODIFY_Clicked(object sender, EventArgs e)
        {
            if (CountdownPick.SelectedIndex == -1 || PropertyPick.SelectedIndex == -1)
            {
               await DisplayAlert("Error", "You attempted to modify without selecting a Countdown or Property to modify!", "OK");
            }
            else
            {
                bool notNull = true;
                List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);

                // Gets Property Pick And Changes The Selected Property Of The Countdown
                switch (PropertyPick.SelectedIndex)
                {
                    case 0:

                        if (NameCountDown.Text != null)
                        {
                            jsonData[CountdownPick.SelectedIndex].Name = NameCountDown.Text;
                        }
                        else
                        {
                            notNull = false;
                        }

                        break;
                    case 1:
                        jsonData[CountdownPick.SelectedIndex].Date = DateCountDown.Date;
                        break;
                    case 2:
                        jsonData[CountdownPick.SelectedIndex].ImagePath = selectedPath;
                        break;
                    case 3:
                        jsonData[CountdownPick.SelectedIndex].VideoPath = selectedPath;
                        break;
                    default:
                        break;
                }

                if (notNull)
                {
                    json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                    File.WriteAllText(fileName, json);

                    await Navigation.PushAsync(new MainMenu());
                    Navigation.RemovePage(this);
                }
                else
                {
                    await DisplayAlert("Error", "You attempted to modify a Countdown with an empty name field, put a name first!", "OK");
                }
            }         
        }

        /// On Loaded Method Goes In Constructor
        private async void OnLoadedAsync()
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                json = await Task.Run(() => r.ReadToEnd());
            }

            List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);
            CountdownPick.ItemsSource = jsonData;          
        }
    }
}