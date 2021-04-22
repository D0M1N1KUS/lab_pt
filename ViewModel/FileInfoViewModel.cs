using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lab3.Commands;
using Lab3.Properties;

namespace Lab3.ViewModel
{
    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        private ImageSource fileIcon = default;
        private readonly string[] _supportedFileTypes = {".txt"};

        public FileInfo _fileInfo;

        public RelayCommand OpenFileCommand { get; private set; }

        public ImageSource FileIcon 
        {
            get => fileIcon ?? (fileIcon = new BitmapImage(new Uri(@"Images/File.png")));
            set => fileIcon = value;
        }

        public override long Size => _fileInfo.Length / 1024L;

        public override string Extension => _fileInfo.Extension;

        public override FileSystemInfo Model
        {
            get => _fileSystemInfo;
            set
            {
                if (_fileSystemInfo == value)
                    return;

                _fileSystemInfo = value;
                _fileInfo = new FileInfo(value.FullName);
                LastWriteTime = value.LastWriteTime;
                Caption = value.Name;
                NotifyPropertyChanged();
            }
        }

        public FileInfoViewModel(ViewModelBase owner) : base(owner)
        {
            OpenFileCommand = new RelayCommand(OpenFileCommandExecute, OpenFileCanExecute);
        }

        private void OpenFileCommandExecute(object obj)
        {
            
        }

        private bool OpenFileCanExecute(object obj)
        {
            var model = obj as FileSystemInfoViewModel;
            if (model == null)
                return false;

            return _supportedFileTypes.Contains( model.Extension);
        }
    }
}