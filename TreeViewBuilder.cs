using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;

namespace Lab1
{
    public class TreeViewBuilder
    {
        public static void BuildDirectoryTreeFrom(string directory, TreeView treeView)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException($"The directory \"{directory}\" does not exist.");

            var rootTreeViewItem = new TreeViewItem {Header = directory};
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
                                .AddMenuItem("Delete")
                                .AddMenuItem("Create new...")
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
                var contextMenuBuilder = new ContextMenuBuilder()
                    .AddMenuItem("Delete");

                if (Path.GetExtension(file).Contains(".txt"))
                    contextMenuBuilder.AddMenuItem("Open");

                var currentTreeViewItem =
                    new TreeViewItemBuilder(Path.GetFileName(file))
                        .SetTag(file)
                        .AddContextMenu(contextMenuBuilder.Build());
                
                current.Items.Add(currentTreeViewItem.Build());
            }
        }
    }
}