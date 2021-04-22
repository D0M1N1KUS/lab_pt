﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Lab3.Sorting.Enums;
using Lab3.ViewModel.Interfaces;

namespace Lab3.ViewModel
{
    public abstract class FileSystemInfoViewModel : ViewModelBase
    {
        protected FileSystemInfo FileSystemInfo;

        private DateTime _lastWriteTime;
        private string _caption;

        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

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

        protected FileSystemInfoViewModel(ViewModelBase owner)
        {
            Owner = owner;
        }

        public bool Equals(FileSystemInfoViewModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(FileSystemInfo, other.FileSystemInfo) && _lastWriteTime.Equals(other._lastWriteTime) && Equals(Items, other.Items) && Caption == other.Caption;
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
                var hashCode = (FileSystemInfo != null ? FileSystemInfo.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _lastWriteTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Items != null ? Items.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Caption != null ? Caption.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}