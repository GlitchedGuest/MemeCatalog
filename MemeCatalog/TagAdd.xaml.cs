using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;

namespace MemeCatalog
{
    /// <summary>
    /// Logika interakcji dla klasy TagAdd.xaml
    /// </summary>
    public partial class TagAdd : Window
    {
        private readonly MemeContext _context = new MemeContext();
        public TagAdd()
        {
            InitializeComponent();
            ItemsRefresh();
        }

        void ItemsRefresh()
        {
            _context.Tags.Load();
            var list = _context.Tags.Local.Select(x => x.Name).ToList();
            ListView.ItemsSource = list;

        }

        private void Tag_Click(object sender, RoutedEventArgs e)
        {
            Tag tag = new Tag { Name = TextBox.Text };

            _context.Tags.Add(tag);
            ItemsRefresh();
            _context.SaveChanges();
        }
    }
}
