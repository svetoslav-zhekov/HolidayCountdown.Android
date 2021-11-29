using System;

namespace HolidayCountdown.CountdownObj
{
    internal class Countdown
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }

        public Countdown()
        {
        }

        public Countdown(string name, DateTime date, string imagePath, string videoPath)
        {
            this.Name = name;
            this.Date = date;
            this.ImagePath = imagePath;
            this.VideoPath = videoPath;
        }
    }
}
