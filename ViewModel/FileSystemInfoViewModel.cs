using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Lab3.Extensions;
using Lab3.Sorting.Enums;
using Lab3.ViewModel.Interfaces;

namespace Lab3.ViewModel
{
    public abstract class FileSystemInfoViewModel : ViewModelBase
    {
        protected FileSystemInfo FileSystemInfo;

        private DateTime _lastWriteTime;
        private string _caption;
        private string _statusMessage;

        protected virtual string ImageSourceFilename => "File.png";

        public abstract long Size { get; }

        public abstract string Extension { get; }

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

        public virtual FileSystemInfo Model
        {
            get => FileSystemInfo;
            set
            {
                if (FileSystemInfo == value)
                    return;

                FileSystemInfo = value;
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

        public FileExplorer OwnerExplorer
        {
            get
            {
                var owner = Owner;
                while (owner is DirectoryInfoViewModel ownerDirectory)
                {
                    if (ownerDirectory.Owner is FileExplorer explorer)
                        return explorer;
                    owner = ownerDirectory.Owner;
                }

                return null;
            }
        }

        public string StatusMessage
        {
            get => _statusMessage ?? string.Empty;
            set
            {
                if (value == _statusMessage)
                    return;

                _statusMessage = value;
                NotifyPropertyChanged();
            }
        }

        public string ImageSource => $"Images/{ImageSourceFilename}";


        protected FileSystemInfoViewModel(ViewModelBase owner)
        {
            Owner = owner;
        }
    }
}