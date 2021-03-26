﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

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
            TreeViewBuilder.SelectedEventHandler = TreeView_Selected;
            TreeViewBuilder.CreateNewEventHandler = (sender, args) =>
                {
                    TryGetItemInfo(out ItemTag itemInfo, out TreeViewItem item);
                    var form = new Dialog(itemInfo.Path, item, this);
                    form.ShowDialog();
                };
        }

        public void AddItem(TreeViewItem item)
        {
            //((TreeViewItem) ExplorerTreeView.SelectedItem).Items.Add(item);
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
