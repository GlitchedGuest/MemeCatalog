using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MemeCatalog
{
   
    public partial class BrowseMemes : Window
    {
        private readonly MemeContext _context = new MemeContext();

        public BrowseMemes()
        {
            InitializeComponent();
            _context.Memes.Load();
            var list = _context.Memes.Local.Select(x => x.Name).ToList();
            ListView.ItemsSource = list;
        }

        private void Obejrzj_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = ListView.SelectedIndex;
            var memes = _context.Memes.ToList();
            mediaElement.Source = new Uri(memes[selectedIndex].Path);
        }
    }
}
