using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MemeCatalog
{
    /// <summary>
    /// Logika interakcji dla klasy MediaPlayer.xaml
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        private bool dragStarted = false;
        private string path = "";
        private bool pause = false;
        TimeSpan position = TimeSpan.Zero;

        private HashSet<string> videoFormats = new HashSet<string>{ "mp4", "mov" };

        public MediaPlayer()
        {
            InitializeComponent();
        }

        public void LoadMedia(string _path)
        {
            if (_path == "")
                return;
            path = _path;
            ME.Source = new Uri(_path);
            ME.Play();
        }

        public void DeloadMedia()
        {
            ME.Source = null;
        }

        private void MediaOpened(object sender, RoutedEventArgs e)
        {
            if (videoFormats.Contains(path.Substring(path.LastIndexOf('.') + 1, path.Length - 1 - path.LastIndexOf('.'))))
            {
                ControlGrid.Visibility = Visibility.Visible;
                VideoProgressBar.Maximum = ME.NaturalDuration.TimeSpan.Ticks;
                timer.Interval = TimeSpan.FromMilliseconds(10);
                timer.Tick += TimerTick;
                timer.Start();
            }
            else
                ControlGrid.Visibility = Visibility.Collapsed;
        }
        private void TimerTick(object? sender, EventArgs e)
        {                

            if (ME.Source != null)
            {
                
                if (!pause && ME.NaturalDuration.HasTimeSpan)
                {
                    if (ME.Position == ME.NaturalDuration.TimeSpan)
                    {
                        pause = true;
                        position = ME.Position;
                    }
                    if (position != ME.Position)
                    {
                        VideoProgressBar.Value = ME.Position.Ticks;
                        DurationText.Text = $"{FormatTime(ME.Position)}/{FormatTime(ME.NaturalDuration.TimeSpan)}";
                    }
                    ControlGrid.Width = ME.ActualWidth;
                }

            }
            if (!pause)
                PlayButton.Content = "❚❚";
            else
                PlayButton.Content = "▶︎";
        }

        private string FormatTime(TimeSpan timeSpan)
        {
            string sHours = timeSpan.Hours > 0 ? $"{timeSpan.Hours}:" : "";

            string sMinutes = "0:";
            if (timeSpan.Minutes > 0)
                sMinutes = $"{timeSpan.Minutes}:";

            string sSeconds = "00";
            if (timeSpan.Seconds < 10)
                sSeconds = $"0{timeSpan.Seconds}";
            else
                sSeconds = $"{timeSpan.Seconds}";

            return sHours + sMinutes + sSeconds;
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragStarted = false;
            position = ME.Position;
            ME.Play();
            pause = false;
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            pause = true;
            ME.Pause();
            dragStarted = true;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dragStarted)
            {
                ME.Position = new TimeSpan((long)VideoProgressBar.Value);
                DurationText.Text = $"{FormatTime(ME.Position)}/{FormatTime(ME.NaturalDuration.TimeSpan)}";
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (!pause)
                ME.Pause();
            else
            {
                if (ME.Position == ME.NaturalDuration.TimeSpan)
                {
                    ME.Stop();
                    position = ME.Position;
                }
                ME.Play();
            }
               
            pause = !pause;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ME.Volume = VolumeSlider.Value;
        }

        private void ControlGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            ControlGrid.Opacity = 100;
        }

        private void ControlGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            ControlGrid.Opacity = 0;
        }
    }
}
