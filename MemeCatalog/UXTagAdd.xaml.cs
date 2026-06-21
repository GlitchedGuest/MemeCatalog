using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Printing;
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
    /// Logika interakcji dla klasy UXTagAdd.xaml
    /// </summary>
    public partial class UXTagAdd : Window
    {
        private readonly MemeContext _context = new MemeContext();
        private int offsetLeft = -100;
        private int offsetTop = -50;

        public List<Tag> tags = new List<Tag>();
        public List<Tag> loadedTags = new List<Tag>();

        public UXTagAdd()
        {
            InitializeComponent();
            _context.Tags.Load();
            var list = _context.Tags.Local.ToList();
            var i = 0;
            foreach (var item in list)
            {
                CreateButton(item.Name, i,TagButton_Click);
                i++;
            }
            CreateButton("+", i, AddTagButton_Click);
        }
        private void CreateButton(string content, int i, RoutedEventHandler method)
        {
            Button b = new Button();
            b.Background = Brushes.White;
            b.Content = $"{content}";
            b.Height = 39;
            b.Width = 69;
            offsetLeft = i % 4 == 0 ? 0 : offsetLeft + 100;
            offsetTop = i % 4 == 0 ? offsetTop + 50 : offsetTop;
            b.Margin = new Thickness(10 + offsetLeft, 10 + offsetTop, 0, 0);
            b.VerticalAlignment = VerticalAlignment.Top;
            b.HorizontalAlignment = HorizontalAlignment.Left;
            b.Click += method;
            GridButtons.Children.Add(b);
        }
        private void TagButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = _context.Tags.Where(x => x.Name.Equals((sender as Button).Content)).FirstOrDefault();
            if ((sender as Button).Background == Brushes.Gray)
            {
                (sender as Button).Background = Brushes.White;
                if(tag != null)
                    tags.Remove(tag);
            }
            else
            {
                (sender as Button).Background = Brushes.Gray;
                if(tag != null)
                    tags.Add(tag);
            }
        }
        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
