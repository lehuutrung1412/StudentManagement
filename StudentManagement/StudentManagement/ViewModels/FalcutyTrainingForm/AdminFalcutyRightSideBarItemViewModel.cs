using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StudentManagement.ViewModels.FalcutyTrainingForm.AdminFalcutyTrainingFormViewModel;

namespace StudentManagement.ViewModels.FalcutyTrainingForm
{
    public class AdminFalcutyRightSideBarItemViewModel : BaseViewModel
    {
        public FalcutyCard CurrentCard { get => _currentCard; set => _currentCard = value; }
        private FalcutyCard _currentCard;

        public AdminFalcutyRightSideBarItemViewModel()
        {
            CurrentCard = null;
        }

        public AdminFalcutyRightSideBarItemViewModel(FalcutyCard card)
        {
            CurrentCard = card;
        }
    }
}
