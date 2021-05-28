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
using Lab3.Builders;
using Lab3.Localization;
using Lab3.OriginalView;
using Lab3.ViewModel;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FileExplorer _fileExplorer;

        public MainWindow()
        {
            InitializeComponent();
            //TreeViewBuilder.DeleteEventHandler = TreeView_DeleteSelectedItem;
            //TreeViewBuilder.OpenEventHandler = TreeView_OpenFile;
            //TreeViewBuilder.SelectedEventHandler = TreeView_Selected;
            TreeViewBuilder.CreateNewEventHandler = (sender, args) =>
                {
                    TryGetItemInfo(out ItemTag itemInfo, out TreeViewItem item);
                    var form = new Dialog(itemInfo.Path, item, this);
                    form.ShowDialog();
                };

            _fileExplorer = new FileExplorer(StatusBar);
            _fileExplorer.PropertyChanged += _fileExplorer_PropertyChanged;
            _fileExplorer.OnOpenFileRequest += _fileExplorer_OnOpenFileRequest;
            DataContext = _fileExplorer;
        }

        private void _fileExplorer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileExplorer.Lang))
                CultureResources.ChangeCulture(CultureInfo.CurrentUICulture);
        }

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

        private void _fileExplorer_OnOpenFileRequest(object sender, FileInfoViewModel viewModel)
        {
            var content = _fileExplorer.GetFileContent(viewModel);
            if (content is string text)
            {
                var textView = new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap};
                ContentViewer.Content = textView;
            }
        }
    }
}
