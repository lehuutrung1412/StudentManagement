using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class FileManagerRightSideBarViewModel : BaseViewModel
    {
        private object _rightSideBarItemViewModel;

        public object RightSideBarItemViewModel
        {
            get { return _rightSideBarItemViewModel; }
            set
            {
                _rightSideBarItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public FileInfo CurrentFile
        {
            get => _currentFile;
            set
            {
                _currentFile = value;
                RightSideBarItemViewModel = _currentFile == null ? new EmptyStateRightSideBarViewModel() : (object)_currentFile;
            }
        }
        private FileInfo _currentFile;

        public ICommand ShowFileInfo { get; set; }
        

        public FileManagerRightSideBarViewModel()
        {
            ShowFileInfo = new RelayCommand<object>((p) => { return true; }, (p) => ShowFileInfoFunction(p));

            //RightSideBarItemViewModel = new EmptyStateRightSideBarViewModel();
        }

        private void ShowFileInfoFunction(object p)
        {
            CurrentFile = p as FileInfo;
            RightSideBarItemViewModel = CurrentFile;
        }
    }
}
