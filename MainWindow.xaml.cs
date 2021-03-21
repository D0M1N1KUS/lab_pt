using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IOException = System.IO.IOException;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string openedDirectory = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            TreeViewBuilder.DeleteEventHandler = TreeView_DeleteSelectedItem;
            TreeViewBuilder.OpenEventHandler = TreeView_OpenFile;
        }

        private void Menu_File_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog {Description = "Select a directory to browse."};

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                openedDirectory = dlg.SelectedPath;
                RefreshExplorerTreeView();
            }
        }

        private void RefreshExplorerTreeView()
        {
            ExplorerTreeView.Items.Clear();
            TreeViewBuilder.BuildDirectoryTree(openedDirectory, ExplorerTreeView);
        }


        private void TreeView_DeleteSelectedItem(object sender, RoutedEventArgs e)
        {
            if (!TryGetItemInfo(out var itemInfo)) 
                return;

            if (itemInfo.IsFile)
            {
                if(File.Exists(itemInfo.Path))
                    TryDeletingFile(itemInfo);
            }
            else
            {
                TryDeletingDirectory(itemInfo.Path);
            }

            RefreshExplorerTreeView();
        }

        private bool TryGetItemInfo(out ItemTag itemInfo)
        {
            itemInfo = default;

            if (ExplorerTreeView.SelectedItem == null)
            {
                Debug.WriteLine("ExplorerTreeView.SelectedItem is null!");
                return false;
            }

            if (!(ExplorerTreeView.SelectedItem is TreeViewItem))
                return false;

            itemInfo = (ItemTag) ((TreeViewItem) ExplorerTreeView.SelectedItem).Tag;
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
            if (!TryGetItemInfo(out ItemTag itemInfo))
                return;

            TextViewer.Text = File.ReadAllText(itemInfo.Path);
        }
    }
}
