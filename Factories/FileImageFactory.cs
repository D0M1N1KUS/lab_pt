using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Lab1.Factories
{
    public static class FileImageFactory
    {
        private const string ApplicationStr = "Application";
        private const string FileStr = "AnyFile";
        private const string MediaFileStr = "MediaFile";
        private const string TextFileStr = "TestFile";
        private const string PictureStr = "Picture";

        private static Dictionary<string, ImageSource> fileIconFDictionary =
            new Dictionary<string, ImageSource>
            {
                [TextFileStr] = new BitmapImageBuilder(@"pack://application:,,,/Images/TextFile.png").Build(),
                [FileStr] = new BitmapImageBuilder(@"pack://application:,,,/Images/File.png").Build(),
                [ApplicationStr] = new BitmapImageBuilder(@"pack://application:,,,/Images/Application.png").Build(),
                [MediaFileStr] = new BitmapImageBuilder(@"pack://application:,,,/Images/MediaFile.png").Build(),
                [PictureStr] = new BitmapImageBuilder(@"pack://application:,,,/Images/PictureJPG.png").Build(),
            };

        public static ImageSource Get(string fileExtension)
        {
            return fileIconFDictionary[GetIconPath(fileExtension)];
        }

        private static string GetIconPath(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".txt":
                    return TextFileStr;
                case ".exe":
                    return ApplicationStr;
                case ".mp3":
                case ".mov":
                case ".avi":
                case ".mp4":
                case ".wav":
                case ".flac":
                    return MediaFileStr;
                case ".jpg":
                case ".bmp":
                case ".png":
                    return PictureStr;
                default:
                    return FileStr;
            }
        }
    }
}