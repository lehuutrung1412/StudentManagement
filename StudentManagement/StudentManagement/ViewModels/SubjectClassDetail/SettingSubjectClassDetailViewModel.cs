using StudentManagement.Commands;
using StudentManagement.Models;
using StudentManagement.Objects;
using StudentManagement.Services;
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

        public ICommand SaveSettingCommand { get; set; }

        private ObservableCollection<ComponentScoreInSetting> _listComponentScore;
        public ObservableCollection<ComponentScoreInSetting> ListComponentScore { get => _listComponentScore; set { _listComponentScore = value; OnPropertyChanged(); } }

        SubjectClass SubjectClassDetail { get; set; }

        public SettingSubjectClassDetailViewModel(SubjectClass subjectClass)
        {
            SubjectClassDetail = subjectClass;

            FirstLoadData();

            AddItemCommand = new RelayCommand<object>((p) => { return true; }, (p) => AddItem());
            SaveSettingCommand = new RelayCommand<object>((p) => { return true; }, (p) => SaveSettingFunction());
            DeleteItemCommand = new RelayCommand<TextBox>((p) => { return true; }, (p) => DeleteItem(p));
        }

        private void FirstLoadData()
        {
            try
            {
                ListComponentScore = new ObservableCollection<ComponentScoreInSetting>();
                var scores = ScoreServices.Instance.LoadComponentScoreOfSubjectClass(SubjectClassDetail.Id);
                foreach (var score in scores)
                {
                    ListComponentScore.Add(ScoreServices.Instance.ConvertComponentScoreToScoreInSetting(score));
                }
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể tải điểm thành phần", "Tải tài nguyên", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }

        }

        private async void SaveSettingFunction()
        {
            try
            {
                if (MyMessageBox.Show("Bạn có muốn lưu cài đặt không?", "Lưu cài đặt", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question)
                    == System.Windows.MessageBoxResult.Yes)
                {
                    await ScoreServices.Instance.DeleteComponentScoreBySubjectClassAsync(SubjectClassDetail.Id);

                    foreach (var score in ListComponentScore.ToList())
                    {
                        if (string.IsNullOrWhiteSpace(score.DisplayName) || string.IsNullOrWhiteSpace(score.Percent.ToString()))
                        {
                            ListComponentScore.Remove(score);
                        }
                        else
                        {
                            await ScoreServices.Instance.SaveComponentScoreDatabaseAsync(score);
                        }
                    }
                    MyMessageBox.Show("Lưu cài đặt thành công", "Lưu cài đặt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                } 
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể lưu cài đặt", "Lưu cài đặt", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void DeleteItem(TextBox p)
        {
            try
            {
                if (p.DataContext == null)
                    return;
                var score = p.DataContext as ComponentScoreInSetting;
                ListComponentScore.Remove(score);
            }
            catch (Exception)
            {
                MyMessageBox.Show("Đã có lỗi xảy ra! Không thể xóa điểm thành phần", "Xóa điểm thành phần", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
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
