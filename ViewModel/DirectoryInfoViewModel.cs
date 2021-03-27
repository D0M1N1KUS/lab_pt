using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Lab1.ViewModel
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        private FileSystemWatcher _fileSystemWatcher = default;

        public Exception LastException { get; private set; }

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
                var itemViewModel = CreateDirectoryViewModel(dirName);
                itemViewModel.Open(dirName);
                Items.Add(itemViewModel);
            }
        }

        private static DirectoryInfoViewModel CreateDirectoryViewModel(string dirName)
        {
            var dirInfo = new DirectoryInfo(dirName);
            var itemViewModel = new DirectoryInfoViewModel {Model = dirInfo};
            return itemViewModel;
        }

        private void AddFilesToItems(string path)
        {
            foreach (var fileName in Directory.GetFiles(path))
            {
                var fileInfo = new FileInfo(fileName);
                var itemViewModel = new FileInfoViewModel {Model = fileInfo};
                Items.Add(itemViewModel);
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
                    Items.Add(new FileInfoViewModel { Model = new FileInfo(fse.FullPath) });
                else if (Directory.Exists(fse.FullPath))
                {
                    var divm = new DirectoryInfoViewModel {Model = new DirectoryInfo(fse.FullPath)};
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
    }
}