using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Objects
{
    public class BaseObjectWithBaseViewModel : BaseViewModel
    {
        public void RunOnPropertyChanged()
        {
            foreach (PropertyInfo propertyInfo in GetType().GetProperties())
            {
                OnPropertyChanged(propertyInfo.Name);
            }
        }
    }
}
