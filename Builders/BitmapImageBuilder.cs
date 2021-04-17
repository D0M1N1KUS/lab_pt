using System;
using System.Windows.Media.Imaging;

namespace Lab3.Builders
{
    public class BitmapImageBuilder
    {
        private readonly string _path;

        public BitmapImageBuilder(string path)
        {
            _path = path;
        }

        public BitmapImage Build()
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri(_path);
            bi.EndInit();

            return bi;
        }
    }
}