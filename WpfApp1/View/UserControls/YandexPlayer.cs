
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using TagLib;
using TagLib.Ape;
using Windows.UI.Xaml.Controls;
using WpfApp1.View.CustomControls;
using static Uno.WinRTFeatureConfiguration;


namespace WpfApp1.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для MediaPlayer.xaml
    /// </summary>
    public partial class YandexPlayer : System.Windows.Controls.UserControl
    {
        private int currentIndex = -1;
        private DispatcherTimer timer;
        bool isPlayed = false;
        bool isOpened = false;
        bool isRepeat = false;
        bool isBluetooth = false;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private TimeSpan TotalTime;


        private List<MediaItem> mp3Files = new List<MediaItem>();

        public YandexPlayer()
        {
            InitializeComponent();
            DataContext = this;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
        }

        #region Methods


        private void Visible()
        {
            if (TrackList.Visibility == Visibility.Collapsed)
            {
                TrackList.Visibility = Visibility.Visible;
            }
            else
            {
                TrackList.Visibility = Visibility.Collapsed;
            }
        }

        private void GetInfo(string filePath)
        {
            mediaPlayer.Open(new Uri(filePath));
            timer.Stop();
            PlayIcon.Visibility = Visibility.Visible;
            PauseIcon.Visibility = Visibility.Collapsed;
            SliderTrack.Value = 0;
            TagLib.File tf = TagLib.File.Create(filePath);
            string artist = tf.Tag.Title;
            Artist.Text = artist;
            string title = tf.Tag.FirstPerformer;
            Title.Text = title;
            double getDurationTrack = tf.Properties.Duration.TotalSeconds;
            double sliderEmd = tf.Properties.Duration.TotalSeconds;
            string endTrack = TimeSpan.FromSeconds(getDurationTrack).ToString(@"mm\:ss");

            EndTrack.Text = endTrack;


            // Load you image data in MemoryStream
            TagLib.IPicture pic = tf.Tag.Pictures[0];
            MemoryStream ms = new MemoryStream(pic.Data.Data);
            ms.Seek(0, SeekOrigin.Begin);

            // ImageSource for System.Windows.Controls.Image
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            TrackImg.Source = bitmap;
            Background.Source = bitmap;

            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {

                StartTrack.Text = FormatTime(mediaPlayer.NaturalDuration.TimeSpan);
            }
            else
            {
                StartTrack.Text = "00:00";
            }

            if (!mp3Files.Contains(mp3Files.FirstOrDefault(p => p.Title == title && p.Author == artist)))
            {
                mp3Files.Add(new MediaItem
                {
                    Author = artist,
                    Title = title,
                    Bitmap = bitmap,
                    FilePath = filePath,
                });
            }
        }

        private void UpdateDisplay()
        {
            string filePath;
            if (currentIndex <= -1 || mp3Files.Count == 0)
            {
                Console.WriteLine("No items.");
            }
            else
            {
               filePath  = mp3Files[currentIndex].FilePath;
                GetInfo(filePath);
            }
        }

        private void Repeating()
        {
            if(isRepeat == true)
            {
                mediaPlayer.Play();
            }
        }
        #endregion

        #region Buttons
        private void ShowList(object sender, RoutedEventArgs e)
        {
            Visible();
            ListViewTracks.Items.Clear();
            mp3Files.ForEach(p =>
            {
                ListViewTracks.Items.Add(p);
            });
        }
        private void NextTrack(object sender, RoutedEventArgs e)
        {
            PlayIcon.Visibility = Visibility.Visible;
            PauseIcon.Visibility = Visibility.Collapsed;
            timer.Stop();
            mediaPlayer.Stop();
            if (mp3Files.Count == 0) return;

            currentIndex++;

            if (currentIndex >= mp3Files.Count)
            {
                currentIndex = 0;
            }
            UpdateDisplay();
        } 
        
        private void PastTrack(object sender, RoutedEventArgs e)
        {
            PlayIcon.Visibility = Visibility.Visible;
            PauseIcon.Visibility = Visibility.Collapsed;
            timer.Stop();
            mediaPlayer.Stop();
            if (mp3Files.Count == 0) return;

            currentIndex--;

            if (currentIndex >= mp3Files.Count || currentIndex <= mp3Files.Count)
            {
                currentIndex = 0;
            }
            UpdateDisplay();
        }  
        private void Repeat(object sender, RoutedEventArgs e)
        {
            if (isRepeat == false)
            {
                isRepeat = true;
                RepeatOff.Visibility = Visibility.Collapsed;
                RepeatOn.Visibility = Visibility.Visible;
            }
            else
            {
                RepeatOff.Visibility = Visibility.Visible;
                RepeatOn.Visibility = Visibility.Collapsed;
                isRepeat = false;
            }
        }
        private void BluetoothEnable(object sender, RoutedEventArgs e)
        {
            if (isBluetooth == false)
            {
                isBluetooth = true;
                BluetoothOff.Visibility = Visibility.Collapsed;
                BluetoothOn.Visibility = Visibility.Visible;
            }
            else
            {
                BluetoothOff.Visibility = Visibility.Visible;
                BluetoothOn.Visibility = Visibility.Collapsed;
                isBluetooth = false;
            }
        }



        private void AddTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                GetInfo(filePath);
            }
        }
       
        private void OpenSpeedSettings(object sender, RoutedEventArgs e)
        {
            if (isOpened == false)
            {
                isOpened = true;
                SpeedWindow.Visibility = Visibility.Visible;
            }
            else
            {
                SpeedWindow.Visibility = Visibility.Collapsed;
                isOpened = false;
            }
        }
        private void Play(object sender, RoutedEventArgs e)
        {
            if (isPlayed == false)
            {
                isPlayed = true;
                mediaPlayer.Play();
                timer.Start();
                PlayIcon.Visibility = Visibility.Collapsed;
                PauseIcon.Visibility = Visibility.Visible;
            }
            else
            {
                PlayIcon.Visibility = Visibility.Visible;
                PauseIcon.Visibility = Visibility.Collapsed;
                isPlayed = false;
                mediaPlayer.Pause();
                timer.Stop();
            }
        }

        private void PlayOnList(object sender, RoutedEventArgs e)
        {

                string filePath = (ListViewTracks.SelectedItem as MediaItem).FilePath;
                GetInfo(filePath);
                TrackList.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region Sliders
        private void SliderTrack_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaPlayer.Source != null)
            {
                mediaPlayer.Position = TimeSpan.FromSeconds(e.NewValue);
            }

            if (SliderTrack.Value == SliderTrack.Maximum)
            {
                SliderTrack.Value = 0;
                mediaPlayer.Stop();
                Repeating();
            }
        }



        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            mediaPlayer.SpeedRatio = (double)SpeedSlider.Value;
        }

        private void OpenEqualizer(object sender, RoutedEventArgs e)
        {
            if(isOpened == false)
            {
                isOpened = true;
                EqualizerWindow.Visibility = Visibility.Visible;
            }
            else
            {
                isOpened = false;
                EqualizerWindow.Visibility = Visibility.Collapsed;  
            }
        }
        #endregion

        #region Timer
        void timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null)
            {
                SliderTrack.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                SliderTrack.Value = mediaPlayer.Position.TotalSeconds;
                StartTrack.Text = FormatTime(mediaPlayer.Position);
            }
        }
        private string FormatTime(TimeSpan time)
        {
            return $"{time.Minutes:D2}:{time.Seconds:D2}";
        }
        #endregion

        public class MediaItem
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public string FilePath { get; set; }
            public BitmapSource Bitmap { get; set; }

        }
    }
}
