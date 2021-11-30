using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class NewsfeedRightSideBarViewModel : BaseViewModel
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
                    IsMakeUpDay = true;

                    IsEvent = AbsentTimes.Contains(_selectedDate) || MakeUpTimes.Contains(_selectedDate);

                    IsAbsentDay = ScheduleTimes.Contains(_selectedDate);
                }
                catch (Exception)
                {
                    IsAbsentDay = false;
                    IsMakeUpDay = true;
                    IsEvent = false;
                }

                if (_selectedDate < DateTime.Now)
                {
                    IsAbsentDay = false;
                    IsMakeUpDay = false;
                    IsEvent = false;
                }

                OnPropertyChanged();
            }
        }
        

        public bool IsEvent { get => _isEvent; set { _isEvent = value; OnPropertyChanged(); } }
        public bool IsAbsentDay { get => _isAbsentDay; set { _isAbsentDay = value; OnPropertyChanged(); } }
        public bool IsMakeUpDay { get => _isMakeUpDay; set { _isMakeUpDay = value; OnPropertyChanged(); } }
        public string PeriodMakeUp { get => _periodMakeUp; set { _periodMakeUp = value; OnPropertyChanged(); } }
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

        public NewsfeedRightSideBarViewModel()
        {
            DisplayDate = DateTime.Now;
            SelectedDate = DateTime.Now;
            IsMakeUpDay = true;

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

            AddAbsentDay = new RelayCommand<object>((p) => true, (p) => AddAbsentDayFunction());
            AddMakeUpDay = new RelayCommand<object>((p) => true, (p) => AddMakeUpDayFunction());
            DeleteEvent = new RelayCommand<object>((p) => true, (p) => DeleteEventFunction());
            CancelAddMakeUpDay = new RelayCommand<object>((p) => true, (p) => CancelAddMakeUpDayFunction());
        }

        private void CancelAddMakeUpDayFunction()
        {
            AddMakeUpMode = false;
            IsMakeUpDay = true;
            IsEvent = AbsentTimes.Contains(_selectedDate) || MakeUpTimes.Contains(_selectedDate);
            IsAbsentDay = ScheduleTimes.Contains(_selectedDate);
        }

        private void DeleteEventFunction()
        {
            try
            {
                if (AbsentTimes.Contains(SelectedDate))
                {
                    AbsentTimes.Remove(SelectedDate);
                }
                if (MakeUpTimes.Contains(SelectedDate))
                {
                    MakeUpTimes.Remove(SelectedDate);
                }
                if (Math.Abs((SelectedDate - ScheduleTimes[0]).Days) % 7 == 0)
                {
                    ScheduleTimes.Add(SelectedDate);
                }
                IsEvent = false;
                RefreshCalendar();
                MyMessageBox.Show("Xóa sự kiện thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception) { }
        }

        private void SwitchToAddMakeUpMode()
        {
            AddMakeUpMode = true;
            IsEvent = false;
            IsAbsentDay = false;
            IsMakeUpDay = false;
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
                    AddMakeUpMode = !AddMakeUpMode;
                    IsEvent = false;
                    IsAbsentDay = false;
                    IsMakeUpDay = false;
                    return;
                }
                AddMakeUpMode = false;
                MakeUpTimes.Add(SelectedDate);
                RefreshCalendar();
                MyMessageBox.Show("Thêm lịch học bù thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception) { }
        }

        private void AddAbsentDayFunction()
        {
            try
            {
                AbsentTimes.Add(SelectedDate);
                ScheduleTimes.Remove(SelectedDate);
                MakeUpTimes.Remove(SelectedDate);
                RefreshCalendar();
                MyMessageBox.Show("Thêm lịch nghỉ học thành công!", "Lịch học", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception) { }
        }
    }
}
