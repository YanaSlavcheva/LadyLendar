namespace LadyLendar
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using Windows.Media.SpeechSynthesis;
    using Windows.Storage;
    using Windows.UI.Xaml.Controls;

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            lstMachineFunctions.ItemsSource = periodsData = new ObservableCollection<PeriodInfoItem>();
        }

        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

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
        }

        private async void WriteTimestamp()
        {
            Windows.Globalization.DateTimeFormatting.DateTimeFormatter formatter =
                new Windows.Globalization.DateTimeFormatting.DateTimeFormatter("longtime");

            StorageFile sampleFile = await localFolder.CreateFileAsync("dataFile.txt", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(sampleFile, formatter.Format(DateTimeOffset.Now));
        }

        async void ReadTimestamp()
        {
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync("dataFile.txt");
                String timestamp = await FileIO.ReadTextAsync(sampleFile);
                // Data is contained in timestamp
            }
            catch (Exception)
            {
                // Timestamp not found
            }
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
