using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Services;
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
        public ObservableCollection<AbsentAndMakeUpItem> AbsentAndMakeUpItemsData { get; set; }
        public ObservableCollection<object> AbsentAndMakeUpItems { get; set; }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                try
                {
                    CancelAddMakeUpDayFunction();
                    LoadAbsentAndMakeUpItems();
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
        public SubjectClass SubjectClassDetail { get; set; }

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
        public NewsfeedRightSideBarViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            _errorBaseViewModel = new ErrorBaseViewModel();
            _errorBaseViewModel.ErrorsChanged += ErrorBaseViewModel_ErrorsChanged;

            ScheduleTimes = new ObservableCollection<DateTime>();
            AbsentAndMakeUpItems = new ObservableCollection<object>();
            FirstLoadData();

            LoadAbsentAndMakeUpItems();

            DisplayDate = DateTime.Now;
            SelectedDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            AddAbsentDay = new RelayCommand<object>((p) => true, (p) => AddAbsentDayFunction());
            AddMakeUpDay = new RelayCommand<object>((p) => true, (p) => AddMakeUpDayFunction());
            DeleteEvent = new RelayCommand<object>((p) => true, (p) => DeleteEventFunction());
            CancelAddMakeUpDay = new RelayCommand<object>((p) => true, (p) => CancelAddMakeUpDayFunction());
        }

        private void FirstLoadData()
        {
            try
            {
                var data = AbsentCalendarServices.Instance.GetListAbsentCalendars(SubjectClassDetail.Id);
                AbsentAndMakeUpItemsData = new ObservableCollection<AbsentAndMakeUpItem>();
                data.ForEach(item => AbsentAndMakeUpItemsData.Add(AbsentCalendarServices.Instance.ConvertAbsentCalendarToAbsentItem(item)));
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải lịch báo nghỉ và báo bù!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            try
            {
                DateTime dateStart = (DateTime)SubjectClassDetail.StartDate;
                DateTime dateEnd = (DateTime)SubjectClassDetail.EndDate;
                int weekDay = (int)SubjectClassDetail.WeekDay;
                SchedulePeriod = SubjectClassDetail.Period;

                for (DateTime date = dateStart.AddDays(weekDay); date <= dateEnd.AddDays(7); date = date.AddDays(7))
                {
                    if (AbsentAndMakeUpItemsData.Any(item => item.Date == date))
                    {
                        continue;
                    }
                    ScheduleTimes.Add(date);
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải lịch học!", "Lỗi rồi", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void LoadAbsentAndMakeUpItems()
        {
            AbsentAndMakeUpItems.Clear();

            foreach (var item in AbsentAndMakeUpItemsData)
            {
                if (item.Date == SelectedDate)
                {
                    AbsentAndMakeUpItems.Add(new AbsentAndMakeUpItem(item.Id, item.IdSubjectClass, item.Date, item.Period, item.Type));
                }
            }
        }

        private bool CheckConflictPeriod()
        {
            var listMakeUpInDay = AbsentAndMakeUpItemsData.Where(item => item.Date == SelectedDate && item.Type == "Học bù").ToList();

            foreach (var number in PeriodMakeUp)
            {
                if (SchedulePeriod.Contains(number) && ScheduleTimes.Contains(SelectedDate))
                {
                    return false;
                }
                foreach (var item in listMakeUpInDay)
                {
                    if (item.Period.Contains(number))
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
            IsEvent = AbsentAndMakeUpItemsData.Any(item => item.Date == SelectedDate);
            IsAbsentDay = ScheduleTimes.Contains(_selectedDate);

            if (_selectedDate < DateTime.Now.AddDays(-1))
            {
                IsAbsentDay = false;
                IsMakeUpDay = false;
                IsEvent = false;
            }
        }

        private async void DeleteEventFunction()
        {
            try
            {
                if (AbsentAndMakeUpItemsData.Any(item => item.Date == SelectedDate))
                {
                    var removeLists = AbsentAndMakeUpItemsData.Where(item => item.Date == SelectedDate).ToList();
                    foreach (var item in removeLists)
                    {
                        if (item.Type == "Nghỉ học")
                        {
                            IsAbsentDay = true;
                        }
                        if (item.Type == "Học bù")
                        {
                            IsMakeUpDay = true;
                        }
                        AbsentAndMakeUpItemsData.Remove(item);
                    }

                    await AbsentCalendarServices.Instance.DeleteAbsentCalendarAsync(SubjectClassDetail.Id, SelectedDate);
                }

                if (ScheduleTimes.Count != 0 && Math.Abs((SelectedDate - ScheduleTimes[0]).Days) % 7 == 0)
                {
                    ScheduleTimes.Add(SelectedDate);
                }

                IsEvent = false;
                RefreshCalendar();
                LoadAbsentAndMakeUpItems();

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

        private async void AddMakeUpDayFunction()
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

                var newAbsent = new AbsentAndMakeUpItem(Guid.NewGuid(), SubjectClassDetail.Id, SelectedDate, PeriodMakeUp, "Học bù");
                AbsentAndMakeUpItemsData.Add(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToDatabaseAsync(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToNotification(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToNotificationInfo(newAbsent);

                PeriodMakeUp = "";
                _errorBaseViewModel.ClearErrors(nameof(PeriodMakeUp));
                SwitchBetweenAddMakeUpModeAndNormal();
                RefreshCalendar();
                LoadAbsentAndMakeUpItems();

                MyMessageBox.Show("Thêm lịch học bù thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async void AddAbsentDayFunction()
        {
            try
            {
                // Absent day must be a schedule day
                if (!ScheduleTimes.Contains(SelectedDate))
                {
                    return;
                }

                ScheduleTimes.Remove(SelectedDate);

                var newAbsent = new AbsentAndMakeUpItem(Guid.NewGuid(), SubjectClassDetail.Id, SelectedDate, SubjectClassDetail.Period, "Nghỉ học");
                AbsentAndMakeUpItemsData.Add(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToDatabaseAsync(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToNotification(newAbsent);
                await AbsentCalendarServices.Instance.SaveCalendarToNotificationInfo(newAbsent);

                RefreshCalendar();
                CancelAddMakeUpDayFunction();
                LoadAbsentAndMakeUpItems();

                MyMessageBox.Show("Thêm lịch nghỉ học thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Vui lòng thử lại sau!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }

    public class AbsentAndMakeUpItem
    {
        public Guid Id { get; set; }
        public Guid IdSubjectClass { get; set; }
        public DateTime Date { get; set; }
        public string Period { get; set; }
        public string Type { get; set; }

        public AbsentAndMakeUpItem(Guid id, Guid idSubjectClass, DateTime date, string period, string type)
        {
            IdSubjectClass = idSubjectClass;
            Id = id;
            Date = date;
            Period = period;
            Type = type;
        }
    }
}
