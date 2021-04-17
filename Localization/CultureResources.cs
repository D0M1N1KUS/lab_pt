using System.Globalization;
using System.Windows.Data;

namespace Lab3.Localization
{
    public class CultureResources
    {
        public Strings GetStringsInstance()
        {
            return new Strings();
        }

        private static ObjectDataProvider _provider;

        public static ObjectDataProvider ResourceProvider =>
            _provider ?? (_provider = 
                (ObjectDataProvider) System.Windows.Application.Current.FindResource("Strings"));

        public static void ChangeCulture(CultureInfo culture)
        {
            ResourceProvider.Refresh();
        }
    }
}