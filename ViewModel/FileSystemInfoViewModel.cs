using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Lab3.Sorting.Enums;

namespace Lab3.ViewModel
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
        private FileSystemInfo _fileSystemInfo;
        public FileInfo _fileInfo;
        private DateTime _lastWriteTime;
        private string _caption;

        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

        public string Extension => _fileInfo.Extension;

        public long Size => _fileInfo.Length / 1024L;

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
                _fileInfo = new FileInfo(value.FullName);
                LastWriteTime = value.LastWriteTime;
                Caption = value.Name;
                NotifyPropertyChanged();
            }
        }

        public string Caption
        {
            get => _caption;
            set
            {
                if(_caption == value)
                    return;

                _caption = value;
                NotifyPropertyChanged();
            }
        }

        public bool Equals(FileSystemInfoViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_fileSystemInfo, other._fileSystemInfo) && _lastWriteTime.Equals(other._lastWriteTime) && Equals(Items, other.Items) && Caption == other.Caption;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FileSystemInfoViewModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_fileSystemInfo != null ? _fileSystemInfo.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _lastWriteTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Items != null ? Items.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Caption != null ? Caption.GetHashCode() : 0);
                return hashCode;
            }
        }

        //public int CompareTo(object other)
        //{
        //    if (other == null)
        //        return 0;
        //    if (other is DirectoryInfoViewModel)
        //        return 1;

        //    if ( < )
        //        return -1;
        //    if (thisHashCode == otherHashCode)
        //        return 0;
        //    return 1;
        //}
    }
}