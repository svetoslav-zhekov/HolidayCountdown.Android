using HolidayCountdown.CountdownObj;
using HolidayCountdown.MainMenuPage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace HolidayCountdown
{
    public partial class MainPage : ContentPage
    {
        ///Timer Stuff
        private bool timerState = true;
        private DateTime targetDate = new DateTime();
        private readonly DateTime currentDate = DateTime.Now;

        /// Button Parameters
        private int width = 110;
        private int height = 100;

        /// Json
        private readonly string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.json");
        private string json = "";

        public MainPage()
        {
            InitializeComponent();
            OnLoaded();
        }

        /// On Click, Go To MainMenu
        private async void Menu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainMenu());
            Navigation.RemovePage(this);
        }

        /// DateTime Section 
        private int GetYear()
        {
            DateTime current = DateTime.Now;
            return current.Year;
        }

        private string IfLessThanTen(int num)
        {
            return (num < 10 ? "0" : "") + num; // If Less Than 10, Add 0 Infront
        }

        private string FormatDate(TimeSpan span)
        {
            int days = span.Days;
            int hours = span.Hours;
            int minutes = span.Minutes;
            int seconds = span.Seconds;

            return $"{IfLessThanTen(days)}:{IfLessThanTen(hours)}:{IfLessThanTen(minutes)}:{IfLessThanTen(seconds)}";
        }

        private DateTime CompareDates(DateTime date)
        {
            int comparison = DateTime.Compare(date, currentDate);

            if (comparison < 0) // If Comparison Is Less Than 0, date Has Already Passed, Increase Year By 1
            {
                return new DateTime(date.Year + 1, date.Month, date.Day);
            }
            else
            {
                return date;
            }
        }

        /// Button Creation Section
        
        private void ChangeImageVideo(string imagePath, string mediaPath)
        {
            if (imagePath.Length != 0)
            {
                BackgroundImage.IsVisible = true;
                BackgroundImage.Source = imagePath;
            }
            else
            {
                BackgroundImage.IsVisible = false;
                BackgroundImage.Source = null;
            }

            if (mediaPath.Length != 0)
            {
                BackgroundVideo.IsVisible = true;
                BackgroundVideo.Source = mediaPath;
            }
            else
            {
                BackgroundVideo.IsVisible = false;
                BackgroundVideo.Source = null;
            }
        }

        private void OnClick(object sender, EventArgs e, string holidayName, string imageSource, string mediaSource, DateTime date)
        {   
            BackgroundVideo.Stop();
            ChangeImageVideo(imageSource, mediaSource);

            TimerName.Text = holidayName;
            targetDate = CompareDates(date); // Check If Date Hasnt Already Passed, Increase Year By 1 If It Has
            TimerDate.Text = $"/{targetDate.Day}.{targetDate.Month}.{targetDate.Year}/";

            DateTime currentTime = DateTime.Now;
            TimeSpan span = targetDate - currentTime;
            TimeLeft.Text = FormatDate(span);
        }

        private Button CreateButton(string holidayName, string imageSource, string mediaSource, DateTime date)
        {
            Button button = new Button()
            {
                Text = holidayName,
                WidthRequest = width,
                HeightRequest = height,
                BackgroundColor = Color.White,
                Margin = 6,
                CornerRadius = 10
            };

            button.Clicked += new EventHandler((sender, e) => OnClick(sender, e, holidayName, imageSource, mediaSource, date)); // Passing Addition Parameter ImgName With Lambda Expression
            return button;
        }

        /// Json Section
        private void DisplayData() // Read Json File And Display Json Data
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                json = r.ReadToEnd();
            }

            List<Countdown> jsonData = JsonConvert.DeserializeObject<List<Countdown>>(json);

            foreach (var json in jsonData)
            {
                ButtonStack.Children.Add(CreateButton(json.Name, json.ImagePath, json.VideoPath, json.Date));
            }

            // Update UI With First Element In Json File
            targetDate = CompareDates(jsonData[0].Date);          
            ChangeImageVideo(jsonData[0].ImagePath, jsonData[0].VideoPath);
            TimerName.Text = jsonData[0].Name;
            TimerDate.Text = $"/{targetDate.Day}.{targetDate.Month}.{targetDate.Year}/";
        }

        private Countdown CreatePredefinedCountdown(string name, DateTime dateTime, string imagePath, string videoPath)
        {
            Countdown countdown = new Countdown()
            {
                Name = name,
                Date = dateTime,
                ImagePath = imagePath,
                VideoPath = "ms-appx:///" + videoPath
            };

            return countdown;
        }

        /// On Loaded Method Goes In Constructor
        private void OnLoaded()
        {
            // If The Json File Exists Add All The Data Stored In It, If Not, Create It
            if (File.Exists(fileName))
            {
                DisplayData();
            }
            else
            {
                Countdown newYear = CreatePredefinedCountdown("New Year", new DateTime(GetYear() + 1, 1, 1), "", "newyear_clip.mp4");
                Countdown christmas = CreatePredefinedCountdown("Christmas", new DateTime(GetYear(), 12, 25), "", "winter_clip.mp4");
                Countdown valentines = CreatePredefinedCountdown("Valentines", new DateTime(GetYear(), 2, 14), "", "valentines_clip.mp4");
                Countdown halloween = CreatePredefinedCountdown("Halloween", new DateTime(GetYear(), 10, 31), "", "halloween_clip.mp4");

                List<Countdown> jsonData = new List<Countdown>()
                {
                    newYear, christmas, valentines, halloween
                };

                json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                File.WriteAllText(fileName, json);
                DisplayData();
            }

            //Update Timer Every Second
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan span = targetDate - currentTime;

                Device.BeginInvokeOnMainThread(() =>
                {
                    TimeLeft.Text = FormatDate(span);
                });

                return timerState; // True = Runs Again || False = Stop
            });
        }
    }
}
