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
        }

        private void Menu_File_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog {Description = "Select a directory to browse."};

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TreeViewBuilder.BuildDirectoryTreeFrom(dlg.SelectedPath, ExplorerTreeView);
                openedDirectory = dlg.SelectedPath;
            }
        }


        private void TreeView_DeleteSelectedItem(object sender, RoutedEventArgs e)
        {
            if (ExplorerTreeView.SelectedItem == null)
            {
                Debug.WriteLine("ExplorerTreeView.SelectedItem is null!");
                return;
            }

            if (ExplorerTreeView.SelectedItem is TreeViewItem)
            {
                var item = (TreeViewItem)ExplorerTreeView.SelectedItem;

                File.Exists((string) item.Tag);
            }

        }
    }
}
