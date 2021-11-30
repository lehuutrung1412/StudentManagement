using StudentManagement.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class NewsfeedRightSideBarViewModel : BaseViewModel, INotifyDataErrorInfo
    {
        public ObservableCollection<DateTime> ScheduleTimes { get; set; }
        public ObservableCollection<DateTime> AbsentTimes { get; set; }
        public ObservableCollection<DateTime> MakeUpTimes { get; set; }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                try
                {
                    CancelAddMakeUpDayFunction();
                }
                catch (Exception)
                {
                    IsAbsentDay = false;
                    IsMakeUpDay = false;
                    IsEvent = false;
                }

                OnPropertyChanged();
            }
        }
        public string PeriodMakeUp
        {
            get => _periodMakeUp;
            set
            {
                _periodMakeUp = value;

                // Validation
                _errorBaseViewModel.ClearErrors();

                if (string.IsNullOrWhiteSpace(PeriodMakeUp))
                {
                    _errorBaseViewModel.AddError(nameof(PeriodMakeUp), "Vui lòng nhập tiết học!");
                }

                if (string.IsNullOrWhiteSpace(PeriodMakeUp))
                {
                    _errorBaseViewModel.AddError(nameof(PeriodMakeUp), "Vui lòng nhập tiết học!");
                }

                OnPropertyChanged();
            }
        }
        public bool IsEvent { get => _isEvent; set { _isEvent = value; OnPropertyChanged(); } }
        public bool IsAbsentDay { get => _isAbsentDay; set { _isAbsentDay = value; OnPropertyChanged(); } }
        public bool IsMakeUpDay { get => _isMakeUpDay; set { _isMakeUpDay = value; OnPropertyChanged(); } }
        public bool AddMakeUpMode { get => _addMakeUpMode; set { _addMakeUpMode = value; OnPropertyChanged(); } }
        public DateTime DisplayDate { get => _displayDate; set { _displayDate = value; OnPropertyChanged(); } }

        private DateTime _selectedDate;
        private DateTime _displayDate;
        private bool _isEvent;
        private bool _isAbsentDay;
        private bool _isMakeUpDay;
        private bool _addMakeUpMode;
        private string _periodMakeUp;

        public ICommand AddAbsentDay { get; set; }
        public ICommand AddMakeUpDay { get; set; }
        public ICommand DeleteEvent { get; set; }
        public ICommand CancelAddMakeUpDay { get; set; }

        #region Validation
        private readonly ErrorBaseViewModel _errorBaseViewModel;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool CanAddMakeUpDay => !HasErrors;
        public bool HasErrors => _errorBaseViewModel.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return _errorBaseViewModel.GetErrors(propertyName);
        }

        private void ErrorBaseViewModel_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanAddMakeUpDay));
        }
        #endregion Validation
        public NewsfeedRightSideBarViewModel()
        {
            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            ScheduleTimes = new ObservableCollection<DateTime>();
            AbsentTimes = new ObservableCollection<DateTime>();
            MakeUpTimes = new ObservableCollection<DateTime>();

            DateTime dateStart = new DateTime(2021, 9, 6);
            DateTime dateEnd = new DateTime(2021, 12, 13);
            int weekDay = 1; // Tuesday

            for (DateTime date = dateStart.AddDays(weekDay); date <= dateEnd; date = date.AddDays(7))
            {
                if (AbsentTimes.Contains(date) || MakeUpTimes.Contains(date))
                {
                    continue;
                }
                ScheduleTimes.Add(date);
            }

            DisplayDate = DateTime.Now;
            SelectedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            AddAbsentDay = new RelayCommand<object>((p) => true, (p) => AddAbsentDayFunction());
            AddMakeUpDay = new RelayCommand<object>((p) => true, (p) => AddMakeUpDayFunction());
            DeleteEvent = new RelayCommand<object>((p) => true, (p) => DeleteEventFunction());
            CancelAddMakeUpDay = new RelayCommand<object>((p) => true, (p) => CancelAddMakeUpDayFunction());
        }

        private bool IsValidPeriod(string period)
        {
            // Max period of subject class is 5
            if (period.Length <= 5)
            {
                
            }
            return false;
        }

        private void CancelAddMakeUpDayFunction()
        {
            AddMakeUpMode = false;
            IsMakeUpDay = true;
            IsEvent = AbsentTimes.Contains(_selectedDate) || MakeUpTimes.Contains(_selectedDate);
            IsAbsentDay = ScheduleTimes.Contains(_selectedDate);

            if (_selectedDate < DateTime.Now.AddDays(-1))
            {
                IsAbsentDay = false;
                IsMakeUpDay = false;
                IsEvent = false;
            }
        }

        private void DeleteEventFunction()
        {
            try
            {
                if (AbsentTimes.Contains(SelectedDate))
                {
                    AbsentTimes.Remove(SelectedDate);
                    IsAbsentDay = true;
                }
                if (MakeUpTimes.Contains(SelectedDate))
                {
                    MakeUpTimes.Remove(SelectedDate);
                    IsMakeUpDay = true;
                }

                if (Math.Abs((SelectedDate - ScheduleTimes[0]).Days) % 7 == 0)
                {
                    ScheduleTimes.Add(SelectedDate);
                }

                IsEvent = false;
                RefreshCalendar();

                MyMessageBox.Show("Xóa sự kiện thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void SwitchBetweenAddMakeUpModeAndNormal()
        {
            if (!AddMakeUpMode)
            {
                AddMakeUpMode = true;
                IsEvent = false;
                IsAbsentDay = false;
                IsMakeUpDay = false;
                return;
            }
            CancelAddMakeUpDayFunction();
        }

        private void RefreshCalendar()
        {
            DisplayDate = SelectedDate.AddMonths(1);
            DisplayDate = SelectedDate;
        }

        private void AddMakeUpDayFunction()
        {
            try
            {
                if (!AddMakeUpMode)
                {
                    SwitchBetweenAddMakeUpModeAndNormal();
                    return;
                }
                
                MakeUpTimes.Add(SelectedDate);

                SwitchBetweenAddMakeUpModeAndNormal();
                RefreshCalendar();

                MyMessageBox.Show("Thêm lịch học bù thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void AddAbsentDayFunction()
        {
            try
            {
                // Absent day must be a schedule day
                if (!ScheduleTimes.Contains(SelectedDate))
                {
                    return;
                }

                AbsentTimes.Add(SelectedDate);
                ScheduleTimes.Remove(SelectedDate);

                RefreshCalendar();
                CancelAddMakeUpDayFunction();

                MyMessageBox.Show("Thêm lịch nghỉ học thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
