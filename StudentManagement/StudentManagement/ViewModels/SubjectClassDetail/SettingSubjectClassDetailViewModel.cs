using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace StudentManagement.ViewModels
{
    public class SettingSubjectClassDetailViewModel : BaseViewModel
    {
        public ICommand AddItemCommand { get => _addItemCommand; set => _addItemCommand = value; }

        private ICommand _addItemCommand;

        public ICommand DeleteItemCommand { get => _deleteItemCommand; set => _deleteItemCommand = value; }

        private ICommand _deleteItemCommand;

        private ObservableCollection<ComponentScoreInSetting> _listComponentScore;
        public ObservableCollection<ComponentScoreInSetting> ListComponentScore { get => _listComponentScore; set { _listComponentScore = value; OnPropertyChanged(); } }

        SubjectClass SubjectClassDetail { get; set; }

        public SettingSubjectClassDetailViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            ListComponentScore = new ObservableCollection<ComponentScoreInSetting>();

            AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
            DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
        }

        public void DeleteItem(TextBox p)
        {
            if (p.DataContext == null)
                return;
            ListComponentScore.Remove(p.DataContext as ComponentScoreInSetting);
        }
        public void AddItem()
        {
            ListComponentScore.Add(new ComponentScoreInSetting
            {
                Id = Guid.NewGuid(),
                DisplayName = "",
                IdSubjectClass = SubjectClassDetail.Id
            });
        }
    }
}
