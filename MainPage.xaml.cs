﻿namespace LadyLendar
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Windows.Media.SpeechSynthesis;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            lstMachineFunctions.ItemsSource = periodsData = new ObservableCollection<PeriodInfoItem>();
        }

        public DateTimeOffset NewlyAddedPeriodStartDate { get; set; }

        public ObservableCollection<PeriodInfoItem> periodsData { get; set; }

        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var newlyAddedPeriodStartDate = e.NewDate;

            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var message = "Please, choose period starting date";

            if (newlyAddedPeriodStartDate != new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero))
            {
                message = $"You set your period to start from { newlyAddedPeriodStartDate.ToString("D", CultureInfo.InvariantCulture) }";
                periodsData.Add(new PeriodInfoItem
                {
                    periodStartDateValue = newlyAddedPeriodStartDate,
                    periodStartDateValueToString = newlyAddedPeriodStartDate.ToString("D", CultureInfo.InvariantCulture),
                    periodEndDateValue = newlyAddedPeriodStartDate.AddDays(5),
                    periodEndDateValueToString = newlyAddedPeriodStartDate.AddDays(5).ToString("D", CultureInfo.InvariantCulture)
                });
            }

            var stream = await synth.SynthesizeTextToStreamAsync(message);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();

            this.daysUntilNextPeriod = (int)(DateTime.Now - newlyAddedPeriodStartDate.AddDays(28)).TotalDays;
        }
    }

    public class PeriodInfoItem
    {
        public DateTimeOffset periodStartDateValue { get; set; }

        public string periodStartDateValueToString { get; set; }

        public DateTimeOffset periodEndDateValue { get; set; }

        public string periodEndDateValueToString { get; set; }
    } 
}
