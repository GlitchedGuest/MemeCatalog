using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
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
        private List<Tag> tags = new List<Tag>();
        private WrapPanel wrappanel = new WrapPanel();

        public UXTestWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            //LVMemesList.ItemsSource = _context.Memes.ToList();
            //TestUC._button.Click += BrowseMemesButton_Click;
            CreateButtons();
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
            tags.ForEach(x => newMeme.Tags.Add(_context.Tags.Where(y => y.TagId == x.TagId).ToList()[0]));
            _context.Memes.Add(newMeme);
            _context.SaveChanges();
            mediaElement.Source = null;
            Tb_Path.Text = "";
            Tb_Name.Text = "";
           // LVMemesList.ItemsSource = _context.Memes.Select(x => x.Name).ToList();
        }
        private void AddTagButton_Click(object sender, RoutedEventArgs e)
        {
            UXTagAdd tagWindow = new UXTagAdd();

            tagWindow.ShowDialog();
            tags = tagWindow.tags;

        }

        private void BrowseMemesButton_Click(object sender, RoutedEventArgs e)
        {
            var browse = TBBrowse.Text;
            var memeList = _context.Memes.Select(x => x.Name).Where(x => !x.Contains(browse)).ToList();
            foreach(var i in wrappanel.Children)
            {
                if (memeList.Contains((i as UCMemeButton)._button.Content))
                    (i as UCMemeButton).Visibility = Visibility.Collapsed;
                else
                    (i as UCMemeButton).Visibility = Visibility.Visible;
            }
        }

        private void CreateButtons()
        {
            int offsetLeft = -100;
            int offsetTop = -100;
            
            wrappanel.Orientation = Orientation.Horizontal;
            wrappanel.HorizontalAlignment = HorizontalAlignment.Left;
            wrappanel.VerticalAlignment = VerticalAlignment.Top;

            var memeList = _context.Memes.ToList();

            foreach(var i in memeList)
            {
                UCMemeButton b = new UCMemeButton();
                b.Margin = new Thickness(25, 0, 25, 0);
                b.VerticalAlignment = VerticalAlignment.Top;
                b.HorizontalAlignment = HorizontalAlignment.Left;
                b._button.Content = i.Name;
                wrappanel.Children.Add(b);
            }

            ScrollViewer.Content = wrappanel;
        }

    }
}
