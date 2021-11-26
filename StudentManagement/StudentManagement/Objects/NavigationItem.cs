using StudentManagement.Commands;
using StudentManagement.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentManagement.Objects
{
    public class NavigationItem : BaseViewModel
    {
        public string NavigationHeader { get; set; }
        public bool CanBeExpanded { get; set; }
        public ObservableCollection<NavigationItem> ExpandedItems { get; set; }
        public object NavigationItemViewModel { get; set; }
        public object RightSideBarNavigationItemViewModel { get; set; }
        public ICommand GoToView { get; set; }
        public LayoutViewModel LayoutViewModel { get; set; }
        public string Icon { get; set; }
        public bool IsPressed { get => _isPressed; set { _isPressed = value; OnPropertyChanged(); } }
        private bool _isPressed;

        public NavigationItem(string navigationHeader, bool canBeExpanded, ObservableCollection<NavigationItem> expandedItems, object navigationItemViewModel, object rightSideBarNavigationItemViewModel, LayoutViewModel layoutViewModel, string icon)
        {
            NavigationHeader = navigationHeader;
            CanBeExpanded = canBeExpanded;
            ExpandedItems = expandedItems;
            NavigationItemViewModel = navigationItemViewModel;
            RightSideBarNavigationItemViewModel = rightSideBarNavigationItemViewModel;
            LayoutViewModel = layoutViewModel;
            Icon = icon;
            GoToView = new RelayCommand<object>((_) => true, (_) => GoToViewFunction());
        }

        private void GoToViewFunction()
        {
            ObservableCollection<NavigationItem> navigationItems = LayoutViewModel.NavigationItems;
            foreach (var item in navigationItems)
            {
                item.IsPressed = false;
            }
            IsPressed = true;
            LayoutViewModel.ContentViewModel = NavigationItemViewModel;
            LayoutViewModel.RightSideBar = RightSideBarNavigationItemViewModel;
        }
    }
}
