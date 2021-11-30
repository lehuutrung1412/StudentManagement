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
        public ObservableCollection<Tuple<DateTime, string>> MakeUpTimes { get; set; }
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

                if (!IsValidPeriod(PeriodMakeUp))
                {
                    _errorBaseViewModel.AddError(nameof(PeriodMakeUp), "Tiết học không hợp lệ!");
                }

                if (!CheckConflictPeriod())
                {
                    _errorBaseViewModel.AddError(nameof(PeriodMakeUp), "Tiết học đã bị trùng lịch học!");
                }

                OnPropertyChanged();
            }
        }
        public bool IsEvent { get => _isEvent; set { _isEvent = value; OnPropertyChanged(); } }
        public bool IsAbsentDay { get => _isAbsentDay; set { _isAbsentDay = value; OnPropertyChanged(); } }
        public bool IsMakeUpDay { get => _isMakeUpDay; set { _isMakeUpDay = value; OnPropertyChanged(); } }
        public bool AddMakeUpMode { get => _addMakeUpMode; set { _addMakeUpMode = value; OnPropertyChanged(); } }
        public DateTime DisplayDate { get => _displayDate; set { _displayDate = value; OnPropertyChanged(); } }
        public string SchedulePeriod { get; set; }

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
            MakeUpTimes = new ObservableCollection<Tuple<DateTime, string>>();

            DateTime dateStart = new DateTime(2021, 9, 6);
            DateTime dateEnd = new DateTime(2021, 12, 13);
            int weekDay = 1; // Tuesday
            SchedulePeriod = "123";

            for (DateTime date = dateStart.AddDays(weekDay); date <= dateEnd; date = date.AddDays(7))
            {
                if (AbsentTimes.Contains(date) || MakeUpTimes.Contains(new Tuple<DateTime, string>(date, SchedulePeriod)))
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

        private bool CheckConflictPeriod()
        {
            var listMakeUpInDay = MakeUpTimes.Where(item => item.Item1 == SelectedDate).ToList();

            foreach (var number in PeriodMakeUp)
            {
                if (SchedulePeriod.Contains(number) && ScheduleTimes.Contains(SelectedDate))
                {
                    return false;
                }
                foreach (var item in listMakeUpInDay)
                {
                    if (item.Item2.Contains(number))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsValidPeriod(string period)
        {
            // Max period of subject class is 5
            if (period.Length <= 5)
            {
                try
                {
                    if (period.TrimStart('0') != period)
                    {
                        return false;
                    }

                    int intPeriod = Convert.ToInt32(period);

                    int lastDigit = intPeriod % 10;
                    intPeriod /= 10;

                    if (lastDigit == 0)
                    {
                        lastDigit = 10;
                    }
 
                    while (intPeriod != 0)
                    {
                        int extractDigit = intPeriod % 10;

                        if (extractDigit == 0)
                        {
                            extractDigit = 10;
                        }

                        if (extractDigit != lastDigit - 1)
                        {
                            return false;
                        }

                        lastDigit = extractDigit;
                        intPeriod /= 10;
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        private void CancelAddMakeUpDayFunction()
        {
            AddMakeUpMode = false;
            IsMakeUpDay = true;
            IsEvent = AbsentTimes.Contains(_selectedDate) || MakeUpTimes.Any(item => item.Item1 == SelectedDate);
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
                if (MakeUpTimes.Any(item => item.Item1 == SelectedDate))
                {
                    var removeLists = MakeUpTimes.Where(item => item.Item1 == SelectedDate).ToList();
                    foreach (var item in removeLists)
                    {
                        MakeUpTimes.Remove(item);
                    }
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

                if (!CheckConflictPeriod() || string.IsNullOrWhiteSpace(PeriodMakeUp))
                {
                    MyMessageBox.Show("Tiết học không hợp lệ", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    return;
                }

                MakeUpTimes.Add(new Tuple<DateTime, string>(SelectedDate, PeriodMakeUp));

                PeriodMakeUp = "";
                _errorBaseViewModel.ClearErrors(nameof(PeriodMakeUp));
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
