using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lab3.Properties;

namespace Lab3.ViewModel
{
    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        private ImageSource fileIcon = default;

        public ImageSource FileIcon 
        {
            get => fileIcon ?? (fileIcon = new BitmapImage(new Uri(@"Images/File.png")));
            set => fileIcon = value;
        }

        public long Size { get; set; }
    }
}