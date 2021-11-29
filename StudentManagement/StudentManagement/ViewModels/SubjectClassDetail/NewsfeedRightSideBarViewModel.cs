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

                    IsAbsentDay = ScheduleTimes.Contains(_selectedDate) || MakeUpTimes.Contains(_selectedDate);
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

        private DateTime _selectedDate;

        private bool _isEvent;
        private bool _isAbsentDay;
        private bool _isMakeUpDay;

        public ICommand AddAbsentDay { get; set; }
        public ICommand AddMakeUpDay { get; set; }
        public ICommand DeleteEvent { get; set; }
        

        public NewsfeedRightSideBarViewModel()
        {
            SelectedDate = DateTime.Now;
            IsMakeUpDay = true;

            ScheduleTimes = new ObservableCollection<DateTime> { new DateTime(2021, 11, 28), new DateTime(2021, 11, 21), new DateTime(2021, 12, 21) };
            AbsentTimes = new ObservableCollection<DateTime> { new DateTime(2021, 11, 27), new DateTime(2021, 11, 20), new DateTime(2021, 12, 20) };
            MakeUpTimes = new ObservableCollection<DateTime>();

            AddAbsentDay = new RelayCommand<object>((p) => true, (p) => AddAbsentDayFunction());
            AddMakeUpDay = new RelayCommand<object>((p) => true, (p) => AddMakeUpDayFunction());
            DeleteEvent = new RelayCommand<object>((p) => true, (p) => DeleteEventFunction());
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
            }
            catch (Exception) { }
        }

        private void AddMakeUpDayFunction()
        {
            try
            {
                MakeUpTimes.Add(SelectedDate);
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
            }
            catch (Exception) { }
        }
    }
}
