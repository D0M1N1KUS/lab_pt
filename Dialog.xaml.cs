using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog()
        {
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
            //if()
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
