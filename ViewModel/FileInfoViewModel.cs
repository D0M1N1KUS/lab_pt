using System;
using System.IO;
using Lab3.Commands;
using Lab3.Factories;

namespace Lab3.ViewModel
{
    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        public RelayCommand OpenFileCommand { get; private set; }

        public FileInfo _fileInfo;

        public override long Size => _fileInfo.Length / 1024L;

        public override string Extension => _fileInfo.Extension;

        public override FileSystemInfo Model
        {
            get => FileSystemInfo;
            set
            {
                if (FileSystemInfo == value)
                    return;

                FileSystemInfo = value;
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

        public void ViewText()
        {
            OwnerExplorer.OpenFile(this);
        }

        protected override string ImageSourceFilename => ImageFileNameFactory.Get(Path.GetExtension(Caption));

        private bool OpenFileCanExecute(object obj)
        {
            return OwnerExplorer.OpenFileCommand.CanExecute(obj);
        }

        private void OpenFileCommandExecute(object obj)
        {
            OwnerExplorer.OpenFileCommand.Execute(obj);
        }
    }
}