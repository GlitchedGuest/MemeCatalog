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

        //Variables for filter options
        private bool _filterEnabled = false;
        private HashSet<String> _filterTagSet = new HashSet<string>();
        private HashSet<String> _filterFileType = new HashSet<string>();


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

        private Button CreateTagButton(Tag tag, RoutedEventHandler function, HashSet<string> tagSet, bool color = true)
        {
            Button _button = new Button();

            _button.Margin = new Thickness(15, 10, 10, 10);
            _button.VerticalAlignment = VerticalAlignment.Top;
            _button.HorizontalAlignment = HorizontalAlignment.Left;
            _button.Height = 50;
            _button.Width = 100;
            if(tagSet.Contains(tag.Name) && color)
                _button.Background = Brushes.Gray;
            else
                _button.Background = Brushes.White;
            _button.Content = tag.Name;
            _button.Click += function;

            return _button;
        }
        private Button CreateFileButton(String fileType, RoutedEventHandler function, HashSet<string> fileSet)
        {
            Button _button = new Button();

            _button.Margin = new Thickness(15, 10, 10, 10);
            _button.VerticalAlignment = VerticalAlignment.Top;
            _button.HorizontalAlignment = HorizontalAlignment.Left;
            _button.Height = 50;
            _button.Width = 100;
            if (fileSet.Contains(fileType))
                _button.Background = Brushes.Gray;
            else
                _button.Background = Brushes.White;
            _button.Content = fileType;
            _button.Click += function;

            return _button;
        }
        private void LoadTagsButtons()
        {
            _context.Tags.Load();

            _wrapPanelBrowseToAddTags.Children.Clear();
            _wrapPanelBrowseToAddTags.Orientation = Orientation.Horizontal;
            _wrapPanelBrowseToAddTags.HorizontalAlignment = HorizontalAlignment.Left;
            _wrapPanelBrowseToAddTags.VerticalAlignment = VerticalAlignment.Top;

            var _tagList = _context.Tags.ToList();

            foreach (var i in _tagList)
            {
                _wrapPanelBrowseToAddTags.Children.Add(CreateTagButton(i,TagButtonClick, _tagsSet));
            }
            SV_BrowseTags.Content = _wrapPanelBrowseToAddTags;
            
        }
        private void LoadBrowseTagsButtons()
        {
            _wrapPanelBrowseTags.Children.Clear();
            _wrapPanelBrowseTags.Orientation = Orientation.Horizontal;
            _wrapPanelBrowseTags.HorizontalAlignment = HorizontalAlignment.Left;
            _wrapPanelBrowseTags.VerticalAlignment = VerticalAlignment.Top;

            var _tagList = _context.Tags.ToList();

            foreach (var i in _tagList)
            {
                _wrapPanelBrowseTags.Children.Add(CreateTagButton(i, InspectTagButtonClick, _tagsSet,false));
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
            _wrapPanelBrowseMemes.Children.Clear();
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
                Path = TBlock_Upload_Path.Text
            };
        }
        private void AddToDatabaseButtonClick(object sender, RoutedEventArgs e)
        {
            _meme.Name = TBox_Upload_Name.Text;
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

            LoadMemesButtons();

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
        private void DeleteMemeButtonClick(object sender, RoutedEventArgs e)
        {
            _context.Memes.Remove(_meme);

            MP_InspectMeme.DeloadMedia();
            _buttonReference = null;
            _meme = null;
            LoadBrowseTagsButtons();
            Grid_InspectMeme.Visibility = Visibility.Collapsed;

            _context.SaveChanges();
        }
        private void InspectMemeBackButtonClick(object sender, RoutedEventArgs e)
        {
            if(_meme != null)
                _meme.Tags.Clear();
            var _tags = _context.Tags.Where(x => _tagsSet.Contains(x.Name)).ToList();
            _tags.ForEach(x => _meme.Tags.Add(x));
            _context.SaveChanges();
            _tagsSet.Clear();
            if (Tab_Tags.IsSelected)
                LoadMemesWithTagsButtons();
            _meme = null;
            Grid_InspectMeme.Visibility = Visibility.Collapsed;
            MP_InspectMeme.DeloadMedia();
        }
        private void ExplorerButtonClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer", $"/select, {TBlock_Inspect_FilePath.Text}");
        }
        private void SearchMemesButtonClick(object sender, RoutedEventArgs e)
        {
            var _browse = TBox_SearchBar.Text;
            HashSet<int> _memesIDWithTags = new HashSet<int>();
            foreach(var i in _filterTagSet)
            {
                var pom = _context.Memes.Where(x => x.Tags.Select(x => x.Name).Contains(i)).Select(x => x.MemeId).ToHashSet();
                _memesIDWithTags = _memesIDWithTags.Concat(pom).ToHashSet();
            }
            var _memesWithTags = _filterTagSet.Any() ? _context.Memes.Where(x => _memesIDWithTags.Contains(x.MemeId)) : _context.Memes;
            var _memesFileTypes = _filterFileType.Any() ? _memesWithTags.Where(x => _filterFileType.Contains(x.FileType)) : _memesWithTags;
            var _memeList = _memesFileTypes.Select(x => x.Name).Where(x => x.Contains(_browse)).ToList();
            foreach (Button i in _wrapPanelBrowseMemes.Children)
            {
                if (_memeList.Contains((i.Content as UCMemeButton)._button.Text))
                    i.Visibility = Visibility.Visible;
                else
                    i.Visibility = Visibility.Collapsed;
            }
            Grid_FilterOptions.Visibility = Visibility.Collapsed;
            SV_BrowseMemes.Visibility = Visibility.Visible;
            _filterEnabled = false;
        }
        private void FilterMemesButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_filterEnabled) {
                SV_BrowseMemes.Visibility = Visibility.Collapsed;
                Grid_FilterOptions.Visibility = Visibility.Visible;
                InitFilterButtons();
            }
            else
            {
                SV_BrowseMemes.Visibility = Visibility.Visible;
                Grid_FilterOptions.Visibility = Visibility.Collapsed;
            }
            _filterEnabled = !_filterEnabled;
        }
        private void ClearFiltersButtonClick(object sender, RoutedEventArgs e)
        {
            _filterFileType.Clear();
            _filterTagSet.Clear();
            InitFilterButtons();
        }
        private void InitFilterButtons()
        {
            WrapPanel filterTags = new WrapPanel();
            filterTags.Orientation = Orientation.Horizontal;
            filterTags.HorizontalAlignment = HorizontalAlignment.Left;
            filterTags.VerticalAlignment = VerticalAlignment.Top;

            WrapPanel filterFileTypes = new WrapPanel();
            filterFileTypes.Orientation = Orientation.Horizontal;
            filterFileTypes.HorizontalAlignment = HorizontalAlignment.Left;
            filterFileTypes.VerticalAlignment = VerticalAlignment.Top;

            var _tagList = _context.Tags.ToList();
            var _fileTypes = _context.Memes.Select(x => x.FileType).Distinct().ToList();

            foreach (var i in _tagList)
            {
                filterTags.Children.Add(CreateTagButton(i, FilterTagButtonClick, _filterTagSet, true));
            }

            foreach (var i in _fileTypes)
            {
                filterFileTypes.Children.Add(CreateFileButton(i, FilterFileTypeButtonClick,_filterFileType));
            }
            SV_FilterMemes_Tags.Content = filterTags;
            SV_FilterMemes_FileTypes.Content = filterFileTypes;
        }

        private void FilterFileTypeButtonClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Background == Brushes.Gray)
            {
                (sender as Button).Background = Brushes.White;
                _filterFileType.Remove((sender as Button).Content.ToString());
            }
            else
            {
                (sender as Button).Background = Brushes.Gray;

                _filterFileType.Add((sender as Button).Content.ToString());
            }
            
        }

        private void FilterTagButtonClick(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).Background == Brushes.Gray)
            {
                (sender as Button).Background = Brushes.White;
                _filterTagSet.Remove((sender as Button).Content.ToString());
            }
            else
            {
                (sender as Button).Background = Brushes.Gray;

                _filterTagSet.Add((sender as Button).Content.ToString());
            }

        }

        private void InspectMemeButtonClick(object sender, RoutedEventArgs e)
        {
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
            foreach (Button i in _wrapPanelBrowseToAddTags.Children)
            {
                if (i.Background == Brushes.Gray)
                    i.Background = Brushes.White;
            }
        }
        private void AddNewTagButtonClick(object sender, RoutedEventArgs e)
        {
            if (Grid_Tags.Visibility == Visibility.Visible && TBox_AddTag.Text == "")
                return;
            else if (Grid_Tags.Visibility == Visibility.Collapsed && TBox_Tags_AddTag.Text == "")
                return;
            Tag _newTag = new Tag()
            {
                Name = Grid_Tags.Visibility == Visibility.Visible ? TBox_AddTag.Text : TBox_Tags_AddTag.Text
            };
            _context.Tags.Add(_newTag);
            _context.SaveChanges();

            LoadBrowseTagsButtons();
            LoadTagsButtons();

            TBox_AddTag.Text = "";
            TBox_Tags_AddTag.Text = "";
        }

        //Tags
        private void SearchTagsButtonClick(object sender, RoutedEventArgs e)
        {
            var _browse = TBox_Tags_SearchBar.Text;
            var _tagsList = _context.Tags.Select(x => x.Name).Where(x => x.Contains(_browse)).ToList();
            foreach (Button i in _wrapPanelBrowseTags.Children)
            {
                if (_tagsList.Contains(i.Content))
                    i.Visibility = Visibility.Visible;
                else
                    i.Visibility = Visibility.Collapsed;
            }
        }

        private void InspectTagButtonClick(object sender, RoutedEventArgs e)
        {
            Grid_BrowseTags.Visibility = Visibility.Collapsed;
            _tag = _context.Tags.Where(x => x.Name == (sender as Button).Content).FirstOrDefault();
            TBox_Tags_TagName.Text = _tag.Name;

            LoadMemesWithTagsButtons();
            
            _buttonReference = sender as Button;
            Grid_InspectTag.Visibility = Visibility.Visible;
        }

        private void LoadMemesWithTagsButtons()
        {
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
            LoadBrowseTagsButtons();
            Grid_InspectTag.Visibility = Visibility.Collapsed;
            Grid_BrowseTags.Visibility = Visibility.Visible;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grid_FilterOptions.Visibility = Visibility.Collapsed;
            SV_BrowseMemes.Visibility = Visibility.Visible;
            _filterEnabled = false;
            Grid_Tags.Visibility = Visibility.Collapsed;
            Grid_InspectMeme.Visibility = Visibility.Collapsed;
            _tagsSet.Clear();
            _meme = null;
            
            MP_InspectMeme.DeloadMedia();

            MP_UploadMeme.DeloadMedia();
            TBlock_Upload_Path.Text = "";
            TBox_Upload_Name.Text = "";
            Btn_AddTags.IsEnabled = false;
            Btn_AddToDatabase.IsEnabled = false;

            foreach (Button i in _wrapPanelBrowseToAddTags.Children)
            {
                if (i.Background == Brushes.Gray)
                    i.Background = Brushes.White;
            }
        }
    }
}
