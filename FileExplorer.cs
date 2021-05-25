using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lab3.Commands;
using Lab3.Localization;
using Lab3.Sorting;
using Lab3.ViewModel;

#if DEBUG
#else
using Lab3.Localization;
using System.Windows.Forms;
#endif

namespace Lab3
{
    public class FileExplorer : ViewModelBase
    {
        private readonly string[] _supportedFileTypes = { ".txt", ".ini", ".log" };
        private DirectoryInfoViewModel _root;
        private string _statusMessage;

        public event EventHandler<FileInfoViewModel> OnOpenFileRequest;

        public static SortingOption SortingOption { get; private set; } = new();

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
        public RelayCommand OpenFileCommand { get; private set; }

        public FileExplorer()
        {
            NotifyPropertyChanged(nameof(Lang));

            OpenRootFolderCommand = new RelayCommand(OpenRootFolderExecute);
            SortRootFolderCommand = new RelayCommand(SortExecute, SortCanExecute);
            OpenFileCommand = new RelayCommand(OpenFileCommandExecute, OpenFileCanExecute);

            SortingOption.PropertyChanged += (_, _) => Root.Sort(SortingOption);
        }

        private bool SortCanExecute(object obj) => Root?.Items?.Count > 0;

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if(value == _statusMessage)
                    return;

                _statusMessage = value;
                NotifyPropertyChanged();
            }
        }

        private void SortExecute(object obj)
        {
            var sortingDialog = new SortingDialog(SortingOption);
            sortingDialog.ShowDialog();
        }

        public void OpenRoot(string path)
        {
            var newRoot = new DirectoryInfoViewModel(this);
            newRoot.PropertyChanged += Root_PropertyChanged;
            newRoot.Open(path);
            Root = newRoot;
            StatusMessage = Strings.Status_Ready;
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

        public void OpenFile(FileInfoViewModel model)
        {
            OnOpenFileRequest?.Invoke(this, model);
        }

        public object GetFileContent(FileInfoViewModel viewModel)
        {
            var extension = viewModel.Extension?.ToLower();
            if (_supportedFileTypes.Contains(extension))
            {
                return GetTextFileContent(viewModel);
            }
            return null;
        }

        private object GetTextFileContent(FileInfoViewModel viewModel)
        {
            try
            {
                return File.ReadAllText(viewModel._fileInfo.FullName);
            }
            catch (IOException ioe)
            {
                Debug.WriteLine(ioe);
                return ioe.ToString();
            }
        }

        private async void OpenRootFolderExecute(object parameter)
        {
#if DEBUG
            var path = "D:\\TestFolder";
#else
            var dlg = new FolderBrowserDialog { Description = Strings.MainWindow_Menu_File_OnClick_Select_a_directory_to_browse_ };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            var path = dlg.SelectedPath;
#endif
            await Task.Factory.StartNew(() => { OpenRoot(path); });
        }

        private void OpenFileCommandExecute(object obj)
        {
            (obj as FileInfoViewModel)?.ViewText();
        }

        private bool OpenFileCanExecute(object obj) =>
            obj is FileSystemInfoViewModel model && _supportedFileTypes.Contains(model.Extension);

        private void Root_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "StatusMessage" && sender is FileSystemInfoViewModel viewModel)
                this.StatusMessage = viewModel.StatusMessage;
        }
    }
}