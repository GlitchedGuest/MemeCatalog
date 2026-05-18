using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class MainWindow : Window
    {
        private readonly MemeContext _context = new MemeContext();

        private Meme newMeme;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            mediaElement.Source = new Uri(ofd.FileName);
            
            TextBoxPath.Text = ofd.FileName;
            TextBoxName.Text = ofd.SafeFileName;
        }

        private void Tag_Click(object sender, RoutedEventArgs e)
        {
            TagAdd tagWindow = new TagAdd();

            tagWindow.Show();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            newMeme = new Meme
            {
                Name = TextBoxName.Text,
                Path = TextBoxPath.Text
            };
            _context.Memes.Add(newMeme);
            _context.SaveChanges();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);
        }
    }
}