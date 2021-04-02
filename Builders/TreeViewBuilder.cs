using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Lab1
{
    public class TreeViewBuilder
    {
        public static RoutedEventHandler DeleteEventHandler = default;
        public static RoutedEventHandler CreateNewEventHandler = default;
        public static RoutedEventHandler OpenEventHandler = default;
        public static RoutedEventHandler SelectedEventHandler = default;

        public static void BuildDirectoryTree(string directory, TreeView treeView)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"The directory \"{directory}\" does not exist.");

            var rootTreeViewItem = new TreeViewItemBuilder(directory)
                .SetTag($"{directory}{Path.DirectorySeparatorChar}")
                .AddContextMenu(new ContextMenuBuilder()
                    .AddMenuItem("Create new...", CreateNewEventHandler)
                    .Build())
                .Build();
            AddAllDirectoryContentsFrom(directory + Path.DirectorySeparatorChar, rootTreeViewItem);
            AddAllFilesFrom(directory + Path.DirectorySeparatorChar, rootTreeViewItem);
            treeView.Items.Add(rootTreeViewItem);
        }

        private static void AddAllDirectoryContentsFrom(string path, TreeViewItem current)
        {
            foreach (string directory in Directory.EnumerateDirectories(path))
            {
                var currentDirectory = $"{directory}{Path.DirectorySeparatorChar}";
                var currentTreeViewItem =
                    new TreeViewItemBuilder(Path.GetFileName(Path.GetDirectoryName(currentDirectory)))
                        .SetTag(currentDirectory)
                        .AddContextMenu(
                            new ContextMenuBuilder()
                                .AddMenuItem("Delete", DeleteEventHandler)
                                .AddMenuItem("Create new...", CreateNewEventHandler)
                                .Build())
                        .Build();

                AddAllDirectoryContentsFrom(currentDirectory, currentTreeViewItem);
                AddAllFilesFrom(currentDirectory, currentTreeViewItem);

                current.Items.Add(currentTreeViewItem);
            }
        }

        private static void AddAllFilesFrom(string path, TreeViewItem current)
        {
            foreach (string file in Directory.EnumerateFiles(path))
            {
                bool isOpenable = Path.GetExtension(file).Contains(".txt");
                var contextMenuBuilder = new ContextMenuBuilder()
                    .AddMenuItem("Delete", DeleteEventHandler);

                if (isOpenable)
                    contextMenuBuilder.AddMenuItem("Open", OpenEventHandler);

                var currentTreeViewItem =
                    new TreeViewItemBuilder(Path.GetFileName(file))
                        .SetTag(file)
                        .SetOnSelectedHandler(SelectedEventHandler)
                        .AddContextMenu(contextMenuBuilder.Build());
                
                current.Items.Add(currentTreeViewItem.Build());
            }
        }
    }
}