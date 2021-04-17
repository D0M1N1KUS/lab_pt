using Lab3.Sorting.Enums;
using Lab3.ViewModel;

namespace Lab3.Sorting
{
    public class SortingOption : ViewModelBase
    {
        private SortBy _sortBy;
        private Direction _direction;

        public SortBy SortBy
        {
            get => _sortBy;
            set
            {
                if(value == _sortBy)
                    return;

                _sortBy = value;
                NotifyPropertyChanged(nameof(SortBy));
            }
        }

        public Direction Direction
        {
            get => _direction;
            set
            {
                if (value == _direction)
                    return;

                _direction= value;
                NotifyPropertyChanged(nameof(Direction));
            }
        }

    }
}