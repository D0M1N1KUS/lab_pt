using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lab1.Properties;

namespace Lab1.ViewModel
{
    public class FileInfoViewModel : FileSystemInfoViewModel
    {
        private ImageSource fileIcon = default;

        public ImageSource FileIcon 
        {
            get => fileIcon ?? (fileIcon = new BitmapImage(new Uri(@"Images/File.png")));
            set => fileIcon = value;
        }


    }
}