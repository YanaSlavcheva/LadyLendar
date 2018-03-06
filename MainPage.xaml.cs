﻿namespace LadyLendar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Threading.Tasks;
    using Windows.Media.SpeechSynthesis;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        ////private DateTimeOffset newlyAddedPeriodStartDate = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);
        private IList<PeriodInfoItem> periodData = new List<PeriodInfoItem>();
        //private int periodLength = Int32.MinValue;

        public MainPage()
        {
            this.InitializeComponent();
            ////this.NewlyAddedPeriodStartDate = newlyAddedPeriodStartDate;
            lstMachineFunctions.ItemsSource = periodsData = new ObservableCollection<PeriodInfoItem>();
        }

        public DateTimeOffset NewlyAddedPeriodStartDate { get; set; }

        public ObservableCollection<PeriodInfoItem> periodsData { get; set; }

        ////private async void periodStartsBtn_ClickAsync(object sender, RoutedEventArgs e)
        ////{
        ////    var mediaElement = new MediaElement();
        ////    var synth = new SpeechSynthesizer();
        ////    var message = "Please, choose period starting date";

        ////    if (this.newlyAddedPeriodStartDate != new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero))
        ////    {
        ////        message = $"You set your period to start from { this.newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture) }";
        ////        periodsData.Add(new PeriodInfoItem
        ////        {
        ////            periodStartDateValue = this.newlyAddedPeriodStartDate,
        ////            periodStartDateValueToString = this.newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture),
        ////            periodEndDateValue = this.newlyAddedPeriodStartDate.AddDays(5),
        ////            periodEndDateValueToString = this.newlyAddedPeriodStartDate.AddDays(5).ToString("d", CultureInfo.InvariantCulture)
        ////        });
        ////    }

        ////    var stream = await synth.SynthesizeTextToStreamAsync(message);
        ////    mediaElement.SetSource(stream, stream.ContentType);
        ////    mediaElement.Play();
        ////}

        private async void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var newlyAddedPeriodStartDate = e.NewDate;

            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var message = "Please, choose period starting date";

            if (newlyAddedPeriodStartDate != new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero))
            {
                message = $"You set your period to start from { newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture) }";
                periodsData.Add(new PeriodInfoItem
                {
                    periodStartDateValue = newlyAddedPeriodStartDate,
                    periodStartDateValueToString = newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture),
                    periodEndDateValue = newlyAddedPeriodStartDate.AddDays(5),
                    periodEndDateValueToString = newlyAddedPeriodStartDate.AddDays(5).ToString("d", CultureInfo.InvariantCulture)
                });
            }

            var stream = await synth.SynthesizeTextToStreamAsync(message);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
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
