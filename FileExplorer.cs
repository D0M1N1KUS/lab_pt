using Lab1.ViewModel;

namespace Lab1
{
    public class FileExplorer : ViewModelBase
    {
        public DirectoryInfoViewModel Root { get; set; }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
        }
    }
}