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
using Lab3.Sorting;

namespace Lab3
{
    /// <summary>
    /// Interaction logic for SortingDialog.xaml
    /// </summary>
    public partial class SortingDialog : Window
    {
        public SortingDialog(SortingOption sortingOption)
        {
            DataContext = sortingOption;
            InitializeComponent();
        }
    }
}
