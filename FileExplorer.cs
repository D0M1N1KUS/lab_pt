using System.Globalization;
using Lab1.ViewModel;

namespace Lab1
{
    public class FileExplorer : ViewModelBase
    {
        public DirectoryInfoViewModel Root { get; set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Lang));
        }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
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
    }
}