
using MauiApp1.Models;
using Plugin.AudioRecorder;


namespace MauiApp1.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string id;
        private string text;
        private string description;
        
        private string date = DateTime.Now.ToShortDateString();
        private int importance = 50;
        private string category;

        public bool isPlaying;
        public bool isRecording => recorder.IsRecording;

        private AudioRecorderService recorder;
        private AudioPlayer player;
        private string fileName;
        public EventHandler finishedPlaying;

        public NewItemViewModel()
        {
            id = Guid.NewGuid().ToString();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

            SwitchRecordingCommand = new Command(SwitchRecording);
            SwitchPlayingCommand = new Command(SwitchPlaying);
            AudioInitialize();
        }
        private async void AudioInitialize()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                 status = await Permissions.RequestAsync<Permissions.Microphone>();

            }
            if (status != PermissionStatus.Granted)
            {
                // TODO: notify user about permission
            }

            recorder = new AudioRecorderService();
            player = new AudioPlayer();
            player.FinishedPlaying += StopPlaying;
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(text)
                && !String.IsNullOrWhiteSpace(description);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
        

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
        public int Importance
        {
            get => importance;
            set => SetProperty(ref importance, value);
        }
        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command SwitchRecordingCommand { get; }
        public Command SwitchPlayingCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Id = id,
                Text = Text,
                Description = Description,
                Date = Date,
                Importance = Importance,
                Category = Category
            };

            await DataStoreItems.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        //void StopPlaying(object sender, EventArgs e)
        //{
        //    buttonRecord.isEnabled = true;
        //    buttonRecord.BackgroundColor = Color.FromHex("#7cbb45");
        //    buttonPlay.IsEnabled = true;
        //    buttonPlay.BackgroundColor = Color.FromHex("#7cbb45");

        //}
        // RECORDING

        private void SwitchRecording()
        {
            if (!recorder.IsRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }
        private async void StartRecording()
        {
            fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "audio_" + id);
            recorder.FilePath = fileName;
            await recorder.StartRecording();
        }
        private async void StopRecording()
        {
            await recorder.StopRecording();
        }

        // PLAYING
        private void SwitchPlaying()
        {
            if (!isPlaying)
            {
                StartPlaying();
            }
            else
            {
                StopPlaying();
            }

        }
        private void StartPlaying()
        {
            isPlaying = true;
            player.Play(fileName);
        }
        void StopPlaying()
        {

            isPlaying = false;
            player.Pause();

        }

       private void StopPlaying(object sender, EventArgs e)
        {

            isPlaying = false;
            finishedPlaying?.Invoke(sender, e);

        }
    }
}
