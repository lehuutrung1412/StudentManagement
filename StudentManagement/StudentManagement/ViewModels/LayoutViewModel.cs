using System.Collections.ObjectModel;
using NavigationItem = StudentManagement.Objects.NavigationItem;

namespace StudentManagement.ViewModels
{
    public class LayoutViewModel : BaseViewModel
    {
        // current contentViewModel and rightSideBarViewModel
        private object _contentViewModel;
        private object _rightSideBar;

        public object ContentViewModel
        {
            get => _contentViewModel;
            set
            {
                _contentViewModel = value;
                OnPropertyChanged();
            }
        }

        public object RightSideBar
        {
            get => _rightSideBar;
            set
            {
                _rightSideBar = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<NavigationItem> _navigationItems;

        public ObservableCollection<NavigationItem> NavigationItems { get => _navigationItems; set => _navigationItems = value; }
        
        public LayoutViewModel() { }
    }
}
