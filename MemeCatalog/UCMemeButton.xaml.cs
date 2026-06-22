using MemeCatalogDatabase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemeCatalog
{
    /// <summary>
    /// Logika interakcji dla klasy UCMemeButton.xaml
    /// </summary>
    public partial class UCMemeButton : UserControl
    {

        public Meme meme;
        private MediaPlayer mediaPlayer = new MediaPlayer();

        public UCMemeButton()
        {
            InitializeComponent();
        }

        public void SetMeme(Meme _meme)
        {
            meme = _meme;
            _button.Text = meme.Name;

            mediaPlayer.MediaOpened += new EventHandler(mediaplayer_OpenMedia);
            mediaPlayer.ScrubbingEnabled = true;
            mediaPlayer.Open(new Uri(meme.Path));
            mediaPlayer.Volume = 0;
            mediaPlayer.Position = TimeSpan.FromSeconds(1);
        }

        private void mediaplayer_OpenMedia(object sender, EventArgs e)
        {
            MediaPlayer mediaPlayer = sender as MediaPlayer;
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawVideo(mediaPlayer, new Rect(0, 0, 100, 100));
            drawingContext.Close();

            double dpiX = 1 / 200;
            double dpiY = 1 / 200;
            RenderTargetBitmap bmp = new RenderTargetBitmap(100, 100, dpiX, dpiY, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            _image.Source = bmp;

            mediaPlayer.Close();
        }
    }
}
