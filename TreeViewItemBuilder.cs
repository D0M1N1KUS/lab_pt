using System.Windows.Controls;

namespace Lab1
{
    public class TreeViewItemBuilder
    {
        private TreeViewItem tvi;

        public TreeViewItemBuilder(string header)
        {
            tvi = new TreeViewItem {Header = header};
        }

        public TreeViewItemBuilder SetTag(object tag)
        {
            tvi.Tag = tag;
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