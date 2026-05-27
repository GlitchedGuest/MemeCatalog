using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemeCatalog
{
    /// <summary>
    /// Logika interakcji dla klasy UXTestWindow.xaml
    /// </summary>
    public partial class UXTestWindow : Window
    {
        private readonly MemeContext _context = new MemeContext();

        public UXTestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            mediaElement.Source = new Uri(ofd.FileName);
            Tb_Path.Text = ofd.FileName;
            Tb_Name.Text = ofd.SafeFileName;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Meme newMeme = new Meme
            {
                Name = Tb_Name.Text,
                Path = Tb_Path.Text
            };
            _context.Memes.Add(newMeme);
            _context.SaveChanges();
            mediaElement.Source = null;
            Tb_Path.Text = "";
            Tb_Name.Text = "";
        }
        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            UXTagAdd tagWindow = new UXTagAdd();

            tagWindow.Show();
        }
    }
}
