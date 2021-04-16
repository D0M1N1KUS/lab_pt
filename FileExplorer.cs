using System.Globalization;
using System.Windows.Forms;
using Lab1.Commands;
using Lab1.Localization;
using Lab1.ViewModel;

namespace Lab1
{
    public class FileExplorer : ViewModelBase
    {
        private DirectoryInfoViewModel root;

        public DirectoryInfoViewModel Root
        {
            get => root;
            set
            {
                root = value;
                NotifyPropertyChanged(nameof(Root));
            }
        }

        public RelayCommand OpenRootFolderCommand { get; private set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Lang));
            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecute);
        }

        public void OpenRoot(string path)
        {
            var newRoot = new DirectoryInfoViewModel();
            newRoot.Open(path);
            Root = newRoot;
            //MainWindow.Datacontext = this;
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
            var dlg = new FolderBrowserDialog { Description = Strings.MainWindow_Menu_File_OnClick_Select_a_directory_to_browse_ };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var path = dlg.SelectedPath;
            OpenRoot(path);
        }
    }
}