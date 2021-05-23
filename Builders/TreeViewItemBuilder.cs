using System.IO;
using System.Windows;
using System.Windows.Controls;
using Lab3.OriginalView;

namespace Lab3.Builders
{
    public class TreeViewItemBuilder
    {
        private TreeViewItem tvi;

        public TreeViewItemBuilder(string header)
        {
            tvi = new TreeViewItem {Header = header};
        }

        public TreeViewItemBuilder SetTag(string path)
        {
            if (File.Exists(path))
            {
                tvi.Tag = new ItemTag(path,File.GetAttributes(path), true,
                    path.EndsWith(".txt"));
            }
            else
            {
                tvi.Tag = new ItemTag(path);
            }

            return this;
        }

        public TreeViewItemBuilder SetOnSelectedHandler(RoutedEventHandler handler)
        {
            tvi.Selected += handler;
            return this;
        }

        public TreeViewItemBuilder AddContextMenu(ContextMenu cm)
        {
            tvi.ContextMenu = cm;
            return this;
        }

        public TreeViewItem Build()
        {
            return tvi;
        }
    }
}