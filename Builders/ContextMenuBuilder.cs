using System.Windows;
using System.Windows.Controls;

namespace Lab3.Builders
{
    public class ContextMenuBuilder
    {
        private ContextMenu cm = new ContextMenu();

        public ContextMenuBuilder AddMenuItem(string header, RoutedEventHandler handler = default)
        {
            var mi = new MenuItem {Header = header};
            if (handler != null)
                mi.Click += handler;

            cm.Items.Add(mi);

            return this;
        }

        public ContextMenu Build()
        {
            return cm;
        }
    }
}