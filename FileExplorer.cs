using System;
using System.Globalization;
using System.Windows.Forms;
using Lab3.Commands;
using Lab3.Localization;
using Lab3.Sorting;
using Lab3.ViewModel;

namespace Lab3
{
    public class FileExplorer : ViewModelBase
    {
        public static SortingOption SortingOption { get; private set; } = new();

        private DirectoryInfoViewModel _root;

        public DirectoryInfoViewModel Root
        {
            get => _root;
            set
            {
                _root = value;
                NotifyPropertyChanged(nameof(Root));
            }
        }

        public RelayCommand OpenRootFolderCommand { get; private set; }
        public RelayCommand SortRootFolderCommand { get; private set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Lang));

            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecute);
            SortRootFolderCommand = new RelayCommand(SortExecute, SortCanExecute);
            

            SortingOption.PropertyChanged += (_, _) => Root.Sort(SortingOption);
        }

        private bool SortCanExecute(object obj) => Root?.Items?.Count > 0;

        private void SortExecute(object obj)
        {
            var sortingDialog = new SortingDialog(SortingOption);
            sortingDialog.ShowDialog();
        }

        public void OpenRoot(string path)
        {
            var newRoot = new DirectoryInfoViewModel();
            newRoot.Open(path);
            Root = newRoot;
        }

        public string Lang
        {
            get => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            set
            {
                if (value == null ||
                    CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == value)
                    return;
 
                CultureInfo.CurrentUICulture = new CultureInfo(value);
                NotifyPropertyChanged();
            }
        }

        private void OpenRootFolderExecute(object obj)
        {
#if DEBUG
            var path = "D:\\TestFolder";
#else
            var dlg = new FolderBrowserDialog { Description = Strings.MainWindow_Menu_File_OnClick_Select_a_directory_to_browse_ };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var path = dlg.SelectedPath;
#endif
            OpenRoot(path);
        }
    }
}