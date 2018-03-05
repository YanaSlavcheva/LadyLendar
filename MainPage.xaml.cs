using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LadyLendar
{
    public sealed partial class MainPage : Page
    {
        private DateTimeOffset newlyAddedPeriodStartDate = new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero);
        ////private IList<PeriodInfoItem> periodData = new List<PeriodInfoItem>();

        public MainPage()
        {
            this.InitializeComponent();
            this.NewlyAddedPeriodStartDate = newlyAddedPeriodStartDate;
            lstMachineFunctions.ItemsSource = periodsData = new ObservableCollection<PeriodInfoItem>();

        }

        public DateTimeOffset NewlyAddedPeriodStartDate { get; set; }

        public ObservableCollection<PeriodInfoItem> periodsData { get; set; }

        private async void periodStartsBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var message = "Please, choose period starting date";

            if (this.newlyAddedPeriodStartDate != new DateTimeOffset(DateTime.MinValue, TimeSpan.Zero))
            {
                message = $"You set your period to start from { this.newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture) }";
                periodsData.Add(new PeriodInfoItem
                {
                    dateType = DateType.periodStarts,
                    dateValue = this.newlyAddedPeriodStartDate,
                    dateValueToString = this.newlyAddedPeriodStartDate.ToString("d", CultureInfo.InvariantCulture)
                });
            }

            var stream = await synth.SynthesizeTextToStreamAsync(message);
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            this.newlyAddedPeriodStartDate = e.NewDate;
        }
    }

    public enum DateType
    {
        periodStarts = 1,
        periodEnds = 2
    }

    public class PeriodInfoItem
    {
        public DateType dateType { get; set; }

        public DateTimeOffset dateValue { get; set; }

        public string dateValueToString { get; set; }
    } 
}
