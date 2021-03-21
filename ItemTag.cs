namespace Lab1
{
    public struct ItemTag
    {
        public ItemTag(string path, bool isFile = false, bool isOpenable = false)
        {
            Path = path;
            IsFile = isFile;
            IsOpenable = isOpenable;
        }

        public string Path;
        public bool IsFile;
        public bool IsOpenable;
    }
}