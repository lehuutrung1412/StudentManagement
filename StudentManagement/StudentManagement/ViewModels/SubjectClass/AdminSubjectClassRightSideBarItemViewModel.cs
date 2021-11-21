using StudentManagement.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static StudentManagement.ViewModels.AdminSubjectClassViewModel;
using Microsoft.Win32;

namespace StudentManagement.ViewModels
{
    public class AdminSubjectClassRightSideBarItemViewModel : BaseViewModel
    {
        public SubjectCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private SubjectCard _currentCard;

        public ICommand ClickChangeImageCommand { get; set; }

        public AdminSubjectClassRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminSubjectClassRightSideBarItemViewModel(SubjectCard card)
        {
            CurrentCard = card;

            ClickChangeImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                };
                if (op.ShowDialog() == true)
                {
                }
            });
        }
    }
}
