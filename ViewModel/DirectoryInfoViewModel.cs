using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Lab3.Extensions;
using Lab3.Factories;
using Lab3.Localization;
using Lab3.Sorting;
using Lab3.Sorting.Enums;

namespace Lab3.ViewModel
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        private FileSystemWatcher _fileSystemWatcher = default;

        protected override string ImageSourceFilename => "Folder.png";

        public static Exception LastException { get; private set; }

        public DispatchedObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new DispatchedObservableCollection<FileSystemInfoViewModel>();

        public long Count => Items?.Count ?? 0;

        public override long Size => Items.Count;
        public override string Extension => Caption;

        public DirectoryInfoViewModel(ViewModelBase owner) : base(owner)
        {
            QuickSort<FileSystemInfoViewModel>.ComparisonPredicate = Compare;
            Items.CollectionChanged += Items_CollectionChanged;
        }

        public bool Open(string path)
        {
            var result = false;

            InitializeFileSystemWatcher(path);

            try
            {
                AddDirectoriesRecursively(path);
                AddFilesToItems(path);

                result = true;
            }
            catch (Exception ex)
            {
                LastException = ex;
            }

            return result;
        }

        private void AddDirectoriesRecursively(string path)
        {
            foreach (var dirName in Directory.GetDirectories(path))
            {
                StatusMessage = string.Format(Strings.Status_LoadingFolderContent, dirName);
                var itemViewModel = CreateDirectoryViewModel(dirName);
                itemViewModel.Open(dirName);
                Items.Add(itemViewModel);
            }
        }

        private DirectoryInfoViewModel CreateDirectoryViewModel(string dirName)
        {
            var dirInfo = new DirectoryInfo(dirName);
            var itemViewModel = new DirectoryInfoViewModel(this) {Model = dirInfo};
            return itemViewModel;
        }

        private void AddFilesToItems(string path)
        {
            foreach (var fileName in Directory.GetFiles(path))
            {
                StatusMessage = $"{Strings.Status_LoadingFile} {fileName}";
                var fileInfo = new FileInfo(fileName);
                var itemViewModel = new FileInfoViewModel(this)
                {
                    Model = fileInfo,
                };
                Items.Add(itemViewModel);
#if DEBUG
                Thread.Sleep(100);
#endif
            }
        }

        private void InitializeFileSystemWatcher(string path)
        {
            _fileSystemWatcher?.Dispose();
            _fileSystemWatcher = new FileSystemWatcher(path);
            //_fileSystemWatcher.Changed += DispatchedOnFileSystemChanged;
            _fileSystemWatcher.Created += (s, e) => InvokeDispatched(OnCreated, s, e);
            _fileSystemWatcher.Deleted += (s , e) => InvokeDispatched(OnDelete, s, e);
            _fileSystemWatcher.Renamed += (s, e) => Application.Current.Dispatcher.Invoke(() => OnRename(s, e));

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void InvokeDispatched(FileSystemEventHandler fse, object sender, FileSystemEventArgs fsea)
        {
            Application.Current.Dispatcher.Invoke(() => fse(sender, fsea));
        }

        private void OnCreated(object sender, FileSystemEventArgs fse)
        {
            try
            {
                if (Items.Any(item => item.Caption == fse.Name))
                    return;

                if(File.Exists(fse.FullPath))
                    Items.Add(new FileInfoViewModel(this) { Model = new FileInfo(fse.FullPath)});
                else if (Directory.Exists(fse.FullPath))
                {
                    var divm = new DirectoryInfoViewModel(this) {Model = new DirectoryInfo(fse.FullPath)};
                    divm.Open(fse.FullPath);
                    Items.Add(divm);
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
            }
        }

        private void OnRename(object sender, RenamedEventArgs rea)
        {
            try
            {
                Items.First(item => item.Caption == rea.OldName).Caption = rea.Name;
            }
            catch (Exception ex)
            {
                LastException = ex;
            }
        }

        private void OnDelete(object sender, FileSystemEventArgs fse)
        {
            try
            {
                string directory = Path.GetDirectoryName(fse.FullPath);
                if (string.IsNullOrEmpty(directory))
                    return;

                RemoveDeletedDirectories(directory);
                RemoveDeletedFiles(directory);
            }
            catch (Exception ex)
            {
                LastException = ex;
            }
        }

        private void RemoveDeletedDirectories(string path)
        {
            var currentDirs = Directory.GetDirectories(path);
            var deletedDirsCaptions = Items.Where(item => item is DirectoryInfoViewModel)
                .Where(divm => currentDirs.All(dir => !dir.EndsWith(divm.Caption)))
                .Select(item => item.Caption)
                .ToList();

            RemoveItemsByCaption(deletedDirsCaptions);
        }

        private void RemoveDeletedFiles(string path)
        {
            var currentFiles = Directory.GetFiles(path);
            var deletedFiles = Items.Where(item => item is FileInfoViewModel)
                .Where(fsvm => currentFiles.All(file => !file.EndsWith(fsvm.Caption)))
                .Select(item => item.Caption)
                .ToList();

            RemoveItemsByCaption(deletedFiles);
        }

        private void RemoveItemsByCaption(IEnumerable<string> deletedItems)
        {
            foreach (string item in deletedItems)
            {
                Items.Remove(Items.First(it => item == it.Caption));
            }
        }

        public void Sort(CancellationToken token, DirectoryInfoViewModel current = null)
        {
            while (!token.IsCancellationRequested)
            {
                if (current == null)
                {
                    current = this;
                    continue;
                }

                int directoriesCount = 0;
                foreach (var directory in current.Items.Where(item => item is DirectoryInfoViewModel))
                {
                    Sort(token, (DirectoryInfoViewModel) directory);
                    directoriesCount++;
                }

                StatusMessage = $"{Strings.Status_Sorting} {current.Caption}";

#if DEBUG
                Thread.Sleep(100);
#endif
                if(!token.IsCancellationRequested)
                    QuickSort<FileSystemInfoViewModel>.Sort(current.Items, 0, directoriesCount);
                if(!token.IsCancellationRequested)
                    QuickSort<FileSystemInfoViewModel>.Sort(current.Items, directoriesCount);

                break;
            }
        }

        private int Compare(FileSystemInfoViewModel item1, FileSystemInfoViewModel item2)
        {
            if (item1.GetType() == typeof(FileInfoViewModel) && item2.GetType() == typeof(DirectoryInfoViewModel))
                return 1;
            if (item1.GetType() == typeof(DirectoryInfoViewModel) && item2.GetType() == typeof(FileInfoViewModel))
                return -1;

            int comparisonValue = FileExplorer.SortingOption.SortBy switch
            {
                SortBy.Name => string.CompareOrdinal(item1.Caption, item2.Caption),
                SortBy.Size => item1.Size.CompareTo(item2.Size),
                SortBy.LastModified => item1.LastWriteTime.CompareTo(item2.LastWriteTime),
                SortBy.Extension => string.CompareOrdinal(item1.Extension, item2.Extension),
                _ => 0
            };

            return FileExplorer.SortingOption.Direction == Direction.Ascending
                ? comparisonValue
                : comparisonValue * -1;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in args.NewItems.Cast<FileSystemInfoViewModel>())
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in args.NewItems.Cast<FileSystemInfoViewModel>())
                    {
                        item.PropertyChanged -= Item_PropertyChanged;
                    }
                    break;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "StatusMessage" && sender is FileSystemInfoViewModel viewModel)
                this.StatusMessage = viewModel.StatusMessage;
        }
    }
}