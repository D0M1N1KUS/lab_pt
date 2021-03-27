using System;
using System.IO;

namespace Lab1.ViewModel
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public Exception exception { get; private set; }

        public bool Open(string path)
        {
            var result = false;

            try
            {
                foreach (var dirName in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(dirName);
                    var itemViewModel = new DirectoryInfoViewModel {Model = dirInfo};
                    Items.Add(itemViewModel);
                }

                foreach (var fileName in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(fileName);
                    var itemViewModel = new FileInfoViewModel {Model = fileInfo};
                    Items.Add(itemViewModel);
                }

                result = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return result;
        }
    }
}