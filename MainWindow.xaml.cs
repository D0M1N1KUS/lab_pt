using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Lab1.Localization;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public static object Datacontext
        {
            get => Instance.DataContext;
            set => Instance.DataContext = value;
        }

        private FileExplorer _fileExplorer;

        public MainWindow()
        {
            InitializeComponent();
            TreeViewBuilder.DeleteEventHandler = TreeView_DeleteSelectedItem;
            TreeViewBuilder.OpenEventHandler = TreeView_OpenFile;
            TreeViewBuilder.SelectedEventHandler = TreeView_Selected;
            TreeViewBuilder.CreateNewEventHandler = (sender, args) =>
                {
                    TryGetItemInfo(out ItemTag itemInfo, out TreeViewItem item);
                    var form = new Dialog(itemInfo.Path, item, this);
                    form.ShowDialog();
                };

            _fileExplorer = new FileExplorer();
            _fileExplorer.PropertyChanged += _fileExplorer_PropertyChanged;
            DataContext = _fileExplorer;

            Instance = this;
        }

        private void _fileExplorer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileExplorer.Lang))
                CultureResources.ChangeCulture(CultureInfo.CurrentUICulture);
        }

        //private void Menu_File_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new FolderBrowserDialog { Description = Strings.MainWindow_Menu_File_OnClick_Select_a_directory_to_browse_ };

        //    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        //        return;

        //    var path = dlg.SelectedPath;
        //    _fileExplorer = new FileExplorer();
        //    _fileExplorer.PropertyChanged += _fileExplorer_PropertyChanged;
        //    _fileExplorer.OpenRoot(path);
        //    DataContext = _fileExplorer;
        //}


        private void TreeView_DeleteSelectedItem(object sender, RoutedEventArgs e)
        {
            if (!TryGetItemInfo(out var itemInfo, out TreeViewItem selectedItem))
                return;

            if (itemInfo.IsFile)
            {
                if (File.Exists(itemInfo.Path))
                    if (TryDeletingFile(itemInfo))
                        RemoveSelectedItem(selectedItem);
            }
            else
            {
                if(TryDeletingDirectory(itemInfo.Path))
                    RemoveSelectedItem(selectedItem);
            }
        }

        private static void RemoveSelectedItem(TreeViewItem selectedItem)
        {
            (selectedItem.Parent as TreeViewItem)?.Items.Remove(selectedItem);
        }

        private bool TryGetItemInfo(out ItemTag itemInfo, out TreeViewItem selectedItem)
        {
            itemInfo = default;
            selectedItem = default;

            if (ExplorerTreeView.SelectedItem == null)
            {
                Debug.WriteLine("ExplorerTreeView.SelectedItem is null!");
                return false;
            }

            if (!(ExplorerTreeView.SelectedItem is TreeViewItem))
                return false;

            selectedItem = (TreeViewItem)ExplorerTreeView.SelectedItem;
            itemInfo = (ItemTag)selectedItem.Tag;
            return true;
        }

        private static bool TryDeletingFile(ItemTag itemInfo)
        {
            try
            {
                File.Delete(itemInfo.Path);
                return true;
            }
            catch (Exception ioe)
            {
                Debug.WriteLine(ioe);
                return false;
            }
        }

        private static bool TryDeletingDirectory(string directory)
        {
            try
            {
                Directory.Delete(directory, recursive: true);
                return true;
            }
            catch (Exception ioe)
            {
                Debug.WriteLine(ioe);
                return false;
            }
        }

        private void TreeView_OpenFile(object sender, RoutedEventArgs e)
        {
            if (!TryGetItemInfo(out ItemTag itemInfo, out _))
                return;

            try
            {
                TextViewer.Text = File.ReadAllText(itemInfo.Path);
            }
            catch (Exception ex)
            {
                TextViewer.Text = ex.Message + "\n" + ex.StackTrace;
            }
        }

        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            if (TryGetItemInfo(out ItemTag itemInfo, out _))
            {
                StatusBarTextBlock.Text = (itemInfo.Properties.HasFlag(FileAttributes.ReadOnly) ? "r" : "-") +
                                          (itemInfo.Properties.HasFlag(FileAttributes.Archive) ? "a" : "-") +
                                          (itemInfo.Properties.HasFlag(FileAttributes.System) ? "s" : "-") +
                                          (itemInfo.Properties.HasFlag(FileAttributes.Hidden) ? "h" : "-");
            }
            else
            {
                StatusBarTextBlock.Text = "----";
            }
        }

        private void Menu_Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
