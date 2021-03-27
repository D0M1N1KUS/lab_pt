using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Lab1.ViewModel
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
        private FileSystemInfo _fileSystemInfo;
        private DateTime _lastWriteTime;

        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

        public DateTime LastWriteTime
        {
            get => _lastWriteTime;
            set
            {
                if (_lastWriteTime == value)
                    return;

                _lastWriteTime = value;
                NotifyPropertyChanged();
            }
        }

        public FileSystemInfo Model
        {
            get => _fileSystemInfo;
            set
            {
                if (_fileSystemInfo == value)
                    return;

                _fileSystemInfo = value;
                LastWriteTime = value.LastWriteTime;
                Caption = value.Name;
                NotifyPropertyChanged();
            }
        }

        public string Caption { get; set; }
    }
}