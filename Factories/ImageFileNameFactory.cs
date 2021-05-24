using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Lab3.Builders;

namespace Lab3.Factories
{
    public static class ImageFileNameFactory
    {
        private const string ApplicationStr = "Application";
        private const string FileStr = "AnyFile";
        private const string MediaFileStr = "MediaFile";
        private const string TextFileStr = "TestFile";
        private const string PictureStr = "Picture";

        private static Dictionary<string, string> fileIconFDictionary =
            new Dictionary<string, string>
            {
                [TextFileStr] = "TextFile.png",
                [FileStr] = @"File.png",
                [ApplicationStr] = @"Application.png",
                [MediaFileStr] = @"MediaFile.png",
                [PictureStr] = @"PictureJPG.png",
            };

        public static string Get(string fileExtension)
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