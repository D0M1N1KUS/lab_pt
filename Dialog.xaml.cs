using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab3.Builders;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        private readonly string _path;
        private readonly TreeViewItem _curretnItem;
        private Regex illegalPathCharsRegex = new Regex("[/?<>\\:*|\"]");

        public Dialog(string path, TreeViewItem curretnItem)
        {
            _path = path;
            _curretnItem = curretnItem;
            InitializeComponent();
        }

        private void FileRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileRadioButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (DirectoryRadioButton.IsChecked ?? false)
                DirectoryRadioButton.IsChecked = false;
        }

        private void DirectoryRadioButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FileRadioButton.IsChecked ?? false)
                FileRadioButton.IsChecked = false;
        }


        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if(illegalPathCharsRegex.IsMatch(NameTextBox.Text) ||
               Directory.Exists(_path + NameTextBox.Text) ||
               File.Exists(_path + NameTextBox.Text))
                return;

            try
            {
                CreateItem();
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void CreateItem()
        {
            string itemName = _path + NameTextBox.Text;

            if ((DirectoryRadioButton.IsChecked ?? false))
            {
                Directory.CreateDirectory(itemName);

                _curretnItem.Items.Add(new TreeViewItemBuilder(NameTextBox.Text)
                    .SetTag(itemName)
                    .SetOnSelectedHandler(TreeViewBuilder.SelectedEventHandler)
                    .AddContextMenu(new ContextMenuBuilder()
                        .AddMenuItem("Delete", TreeViewBuilder.DeleteEventHandler)
                        .AddMenuItem("Create new...")
                        .Build())
                    .Build());
            }
            else
            {
                var attr = (FileAttributes) 0;
                if (ReadOnlyCheckbox.IsChecked ?? false)
                    attr |= FileAttributes.ReadOnly;
                if (ArchiveCheckbox.IsChecked ?? false)
                    attr |= FileAttributes.Archive;
                if (SystemCheckbox.IsChecked ?? false)
                    attr |= FileAttributes.System;
                if (HiddenCheckbox.IsChecked ?? false)
                    attr |= FileAttributes.Hidden;

                File.Create(itemName);
                File.SetAttributes(itemName, attr);

                var cmb = new ContextMenuBuilder().AddMenuItem("Delete", TreeViewBuilder.DeleteEventHandler);
                if (NameTextBox.Text.EndsWith(".txt"))
                    cmb.AddMenuItem("Open", TreeViewBuilder.OpenEventHandler);

                _curretnItem.Items.Add(new TreeViewItemBuilder(NameTextBox.Text)
                    .SetTag(itemName)
                    .SetOnSelectedHandler(TreeViewBuilder.SelectedEventHandler)
                    .AddContextMenu(cmb.Build())
                    .Build());
            }
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
