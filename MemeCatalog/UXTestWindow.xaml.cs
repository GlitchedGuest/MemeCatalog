using MemeCatalogDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace MemeCatalog
{
    public partial class UXTestWindow : Window
    {
        //Variables for db operations
        private readonly MemeContext _context = new MemeContext();
        private HashSet<String> _tagsSet = new HashSet<String>();

        //Wrappanel for buttons
        private WrapPanel _wrapPanelBrowseMemes = new WrapPanel();
        private WrapPanel _wrapPanelBrowseToAddTags = new WrapPanel();
        private WrapPanel _wrapPanelBrowseTags = new WrapPanel();

        //Variables for Creating/editing Memes
        private Meme _meme = null;
        private Tag _tag = null;
        private Button _buttonReference = null;


        //App initialize

        public UXTestWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _context.Database.EnsureCreated();
            LoadMemesButtons();
            
            LoadBrowseTagsButtons();
        }

        //Loading buttons

        private Button CreateTagButton(Tag tag, RoutedEventHandler function)
        {
            Button _button = new Button();

            _button.Margin = new Thickness(15, 10, 10, 10);
            _button.VerticalAlignment = VerticalAlignment.Top;
            _button.HorizontalAlignment = HorizontalAlignment.Left;
            _button.Height = 50;
            _button.Width = 100;
            _button.Background = Brushes.White;
            _button.Content = tag.Name;
            _button.Click += function;

            return _button;
        }
        private void LoadTagsButtons()
        {
            _context.Tags.Load();

            _wrapPanelBrowseToAddTags.Orientation = Orientation.Horizontal;
            _wrapPanelBrowseToAddTags.HorizontalAlignment = HorizontalAlignment.Left;
            _wrapPanelBrowseToAddTags.VerticalAlignment = VerticalAlignment.Top;

            var _tagList = _context.Tags.ToList();

            foreach (var i in _tagList)
            {
                _wrapPanelBrowseToAddTags.Children.Add(CreateTagButton(i,TagButtonClick));
            }
            SV_BrowseTags.Content = _wrapPanelBrowseToAddTags;
            
        }
        private void LoadBrowseTagsButtons()
        {
            _wrapPanelBrowseToAddTags.Orientation = Orientation.Horizontal;
            _wrapPanelBrowseToAddTags.HorizontalAlignment = HorizontalAlignment.Left;
            _wrapPanelBrowseToAddTags.VerticalAlignment = VerticalAlignment.Top;

            var _tagList = _context.Tags.ToList();

            foreach (var i in _tagList)
            {
                _wrapPanelBrowseTags.Children.Add(CreateTagButton(i, InspectTagButtonClick));
            }
            SV_Tags_BrowseTags.Content = _wrapPanelBrowseTags;
        }
        private Button CreateMemeButton(Meme meme)
        {
            Button _button = new Button();
            UCMemeButton _UCSButton = new UCMemeButton();

            _button.Margin = new Thickness(15, 10, 10, 10);
            _button.VerticalAlignment = VerticalAlignment.Top;
            _button.HorizontalAlignment = HorizontalAlignment.Left;
            _button.Background = Brushes.Transparent;
            _button.Height = 120;
            _button.Width = 100;
            _UCSButton.SetMeme(meme);
            _button.Content = _UCSButton;
            _button.Click += InspectMemeButtonClick;

            return _button;
        }
        private void LoadMemesButtons()
        {

            _wrapPanelBrowseMemes.Orientation = Orientation.Horizontal;
            _wrapPanelBrowseMemes.HorizontalAlignment = HorizontalAlignment.Left;
            _wrapPanelBrowseMemes.VerticalAlignment = VerticalAlignment.Top;

            var _memeList = _context.Memes.ToList();

            foreach (var i in _memeList)
            {
                _wrapPanelBrowseMemes.Children.Add(CreateMemeButton(i));
            }

            SV_BrowseMemes.Content = _wrapPanelBrowseMemes;
        }

        //Upload Meme Segement

        private void UploadButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _ofd = new OpenFileDialog();
            _ofd.ShowDialog();

            MP_UploadMeme.LoadMedia(_ofd.FileName);
            TBlock_Upload_Path.Text = _ofd.FileName;
            TBox_Upload_Name.Text = _ofd.SafeFileName;
            Btn_AddTags.IsEnabled = true;
            Btn_AddToDatabase.IsEnabled = true;

            _meme = new Meme()
            {
                Name = TBox_Upload_Name.Text,
                Path = TBlock_Upload_Path.Text
            };
        }
        private void AddToDatabaseButtonClick(object sender, RoutedEventArgs e)
        {
            var _tags = _context.Tags.Where(x => _tagsSet.Contains(x.Name)).ToList();
            _tags.ForEach(x => _meme.Tags.Add(x));

            _context.Memes.Add(_meme);
            _context.SaveChanges();

            _tagsSet.Clear();
            MP_UploadMeme.DeloadMedia();
            TBlock_Upload_Path.Text = "";
            TBox_Upload_Name.Text = "";
            Btn_AddTags.IsEnabled = false;
            Btn_AddToDatabase.IsEnabled = false;

            _wrapPanelBrowseMemes.Children.Add(CreateMemeButton(_meme));

            _meme = null;
        }
        private void AddTagButtonClick(object sender, RoutedEventArgs e)
        {
            LoadTagsButtons();
            foreach (Button i in _wrapPanelBrowseToAddTags.Children)
            {
                if (_tagsSet.Contains(i.Content))
                    i.Background = Brushes.Gray;
            }
            Grid_Tags.Visibility = Visibility.Visible;
        }

        //Browse Memes

        private void UpdateMemeButtonClick(object sender, RoutedEventArgs e)
        {
            _meme.Name = TBox_Inspect_FileName.Text;
            (_buttonReference.Content as UCMemeButton)._button.Text = TBox_Inspect_FileName.Text;
            _context.SaveChanges();
        }
        private void UpdateTagsButtonClick(object sender, RoutedEventArgs e)
        {
            LoadTagsButtons();
            foreach (Button i in _wrapPanelBrowseToAddTags.Children)
            {
                if (_tagsSet.Contains(i.Content))
                    i.Background = Brushes.Gray;
            }
            Grid_Tags.Visibility = Visibility.Visible;
        }
        private void DeleteMemeButtonClick(object sender, RoutedEventArgs e)
        {
            _context.Memes.Remove(_meme);

            _wrapPanelBrowseMemes.Children.Remove(_buttonReference);
            MP_InspectMeme.DeloadMedia();
            _buttonReference = null;
            _meme = null;
            Grid_InspectMeme.Visibility = Visibility.Collapsed;
            Grid_BrowseMemes.Visibility = Visibility.Visible;

            _context.SaveChanges();
        }
        private void InspectMemeBackButtonClick(object sender, RoutedEventArgs e)
        {
            _meme.Tags.Clear();
            var _tags = _context.Tags.Where(x => _tagsSet.Contains(x.Name)).ToList();
            _tags.ForEach(x => _meme.Tags.Add(x));
            _context.SaveChanges();
            _tagsSet.Clear();
            _meme = null;
            Grid_InspectMeme.Visibility = Visibility.Collapsed;
            Grid_BrowseMemes.Visibility = Visibility.Visible;
            MP_InspectMeme.DeloadMedia();
        }
        private void ExplorerButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $"/select, {TBlock_Inspect_FilePath.Text}");
        }
        private void SearchMemesButtonClick(object sender, RoutedEventArgs e)
        {
            var _browse = TBox_SearchBar.Text;
            var _memeList = _context.Memes.Select(x => x.Name).Where(x => !x.Contains(_browse)).ToList();
            foreach (var i in _wrapPanelBrowseMemes.Children)
            {
                if (_memeList.Contains((i as UCMemeButton)._button.Text))
                    (i as UCMemeButton).Visibility = Visibility.Collapsed;
                else
                    (i as UCMemeButton).Visibility = Visibility.Visible;
            }
        }
        private void InspectMemeButtonClick(object sender, RoutedEventArgs e)
        {
            Grid_BrowseMemes.Visibility = Visibility.Collapsed;
            Grid_InspectMeme.Visibility = Visibility.Visible;
            _buttonReference = sender as Button;
            _meme = (_buttonReference.Content as UCMemeButton).meme;
            _tagsSet = _meme.Tags.Select(x => x.Name).ToHashSet();
            MP_InspectMeme.LoadMedia(_meme.Path);
            TBox_Inspect_FileName.Text = ((sender as Button).Content as UCMemeButton).meme.Name;
            TBlock_Inspect_FilePath.Text = ((sender as Button).Content as UCMemeButton).meme.Path;
        }

        //Tag Adding

        private void TagButtonClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Background == Brushes.Gray)
            {
                (sender as Button).Background = Brushes.White;
                _tagsSet.Remove((sender as Button).Content.ToString());
            }
            else
            {
                (sender as Button).Background = Brushes.Gray;

                _tagsSet.Add((sender as Button).Content.ToString());
            }
        }
        private void AddTagBackButtonClick(object sender, RoutedEventArgs e)
        {
            Grid_Tags.Visibility = Visibility.Collapsed;
            _wrapPanelBrowseToAddTags.Children.Clear();
            foreach (Button i in _wrapPanelBrowseToAddTags.Children)
            {
                if (i.Background == Brushes.Gray)
                    i.Background = Brushes.White;
            }
        }
        private void AddNewTagButtonClick(object sender, RoutedEventArgs e)
        {
            if (TBox_AddTag.Text == "")
                return;
            Tag _newTag = new Tag()
            {
                Name = TBox_AddTag.Text
            };
            _context.Tags.Add(_newTag);
            _context.SaveChanges();

            _wrapPanelBrowseToAddTags.Children.Add(CreateTagButton(_newTag, TagButtonClick));

            TBox_AddTag.Text = "";

        }

        //Tags

        private void InspectTagButtonClick(object sender, RoutedEventArgs e)
        {
            Grid_BrowseTags.Visibility = Visibility.Collapsed;
            _tag = _context.Tags.Where(x => x.Name == (sender as Button).Content).FirstOrDefault();
            TBox_Tags_TagName.Text = _tag.Name;

            var _memesWithTags = _context.Memes.Where(x => x.Tags.Contains(_tag)).ToList();
            var _wp = new WrapPanel();
            _wp.Orientation = Orientation.Horizontal;
            _wp.HorizontalAlignment = HorizontalAlignment.Left;
            _wp.VerticalAlignment = VerticalAlignment.Top;

            foreach (var i in _memesWithTags)
            {
                _wp.Children.Add(CreateMemeButton(i));
            }
            SV_MemesWithTags.Content = _wp;
            _buttonReference = sender as Button;
            Grid_InspectTag.Visibility = Visibility.Visible;
        }
        private void InspectTagsBackButtonClick(object sender, RoutedEventArgs e)
        {
            Grid_InspectTag.Visibility = Visibility.Collapsed;
            _buttonReference = null;
            _tag = null;
            Grid_BrowseTags.Visibility = Visibility.Visible;
        }

        private void UpdateTagButtonClick(object sender, RoutedEventArgs e)
        {
            _tag.Name = TBox_Tags_TagName.Text;
            _buttonReference.Content = TBox_Tags_TagName.Text;
            _context.SaveChanges();
        }

        private void DeleteTagButtonClick(object sender, RoutedEventArgs e)
        {
            _context.Tags.Remove(_tag);

            (SV_Tags_BrowseTags.Content as WrapPanel).Children.Remove(_buttonReference);

            _buttonReference = null;
            _tag = null;

            _context.SaveChanges();

            Grid_InspectTag.Visibility = Visibility.Collapsed;
            Grid_BrowseTags.Visibility = Visibility.Visible;
        }

    }
}
