using System.IO;

namespace Lab3.OriginalView
{
    public struct ItemTag
    {
        public ItemTag(string path, FileAttributes properties = FileAttributes.Directory, bool isFile = false, bool isOpenable = false)
        {
            Path = path;
            Properties = properties;
            IsFile = isFile;
            IsOpenable = isOpenable;
        }

        public string Path;
        public FileAttributes Properties;
        public bool IsFile;
        public bool IsOpenable;
    }
}